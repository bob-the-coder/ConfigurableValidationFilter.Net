# Chapter 3 - Validation Rules

In this section, we'll be looking over all aspects of Validation Rules: from how they can be defined and used, to how they can be customized to suit one's needs.

They are - quite literally - the bread and butter of validation filters. Validation Rules define the criteria which the validated object should pass. In essence, a validation rule is **some  function** one would call for a **given object**, and this **function** yields some results which indicates that the **object** is valid. 

Going off this reasoning,  we can define the most basic type of rule as being some abstract class `ValidationRule`, which in practice can validate **any type** of object.

## 3.1 Thinking in abstract
```
abstract class ValidationRule
{
    public RuleMetadata Metadata { get; set; }

    public abstract ValidationResult Validate(Object obj, RuleParameters ruleParameters);
}
```

This is a nice abstraction to get us started. This class describes **the most generic type** of validation rule. It knows how to validate an object. If any extra parameters are needed, they can be passed to the `Validate` method in a `RuleParameters` instance.

Information of any kind, relating to the rule or not, can be stored in the `Metadata` property. The `RuleMetadata` class can be extended in order to store such information.

### 3.1.1 Validation Result

Since any operation yields some result, it's only fitting that validation do so too. Given that the point of validating something is to find out whether the thing is valid or not, the most basic example of such a result that I can think of would include a success indicator, and an error message. And since we can, why not include the validated value and any other information?. Something like this:

```
class ValidationResult
{
    public bool Success { get; set; }
    public string ValidatedValue { get; set; }
    public string Error { get; set; }
    public object Metadata { get; set; }
}
```

Notice that this time, the `Metadata` property is a generic `object`. If you want to get type-y with it, this class can also be made generic, like so:

```
class ValidationResult<TMetadata> 
{
    public bool Success { get; set; }
    public string ValidatedValue { get; set; }
    public string Error { get; set; }
    public TMetadata Metadata { get; set; }
}
```

This way, you can define the type of metadata you'd like to use.

### 3.1.2 Rule Parameters

If we think of rules as functions, then these are essentially extra arguments for that function. Some rules might require a lower or upper-limit (min-max) while others might require a specific value. One rule could require no value, and another might want to cherry-pick a list. In practice, rule parameters come in all shapes and sizes. It's nice then, that all we need to cover all this variety is this little snippet.

```
class RuleParameters
{
}
```

As we will soon find out, the type of parameters is only ever needed when defining a rule. In practice, each rule will already know what type of parameters it uses. More on this later.

## 3.2 Rules, Rules, Rules

There's no way around it. It's a rule-based validation filter. We're talking about rules. 

If I were to buy different types of **fruit**, I would apply **some rules**, depending on the type of fruit I want. Say I like medium-sized red apples, extra-jumbo green watermelons, and teeny-tiny purple grapes. What I'm doing is applying different size and color criteria in order to pick the fruit I want. I'm creating fruit rules.

On the other hand, I also want to buy different types of **clothing**. In this case, I'd have different types of rules, and I'd be applying them to a different type of object. Notice however, that I can apply size and color rules to these **clothing** objects just the same as I could for **fruit**.

Since technically speaking, any rule can be applied to any object *(whether it makes sense or not)*, we need to find a way to describe these different pairs which are made up of one **validated object** and **rule**.

## 3.3 A Generic Abstraction

To achieve this, we will make the `ValidationRule` class generic by adding the type parameters `TRules` and `TObject`. This will allow us to specify for any given `ValidationRule` the type of object it can validate, and which type of rule is being applied.

```
abstract class ValidationRule<TRules, TObject>
{
    public RuleMetadata<TRules> Metadata { get; set; }
    public abstract ValidationResult Validate(TObject obj, RuleParameters ruleParameters);
}
```

The **type parameter** `TObject` defines the **type of object** a filter can validate. Simple as that. You want to validate **some type of object** using **some type of rules**? You can do so using an instance of `ValidationRule<SomeRuleType, SomeTypeOfObject>`.

The **type parameter** `TRules` is a bit more hearty. It is primarily used to uniquely identify each rule within a filter. It is also used in identifying the different available rules within a filter. More on this later.

These parameters are a step up, and play an important role in convenience and performance. However, it doens't end here. There are many rules which can be defined for any property of a given object. You can have a rule for **size**, a rule for **color**, another different rule for **size**, and so on.

## 3.4 Of Values and Validation

There's a rule in *Set Theory* that states that a **Set** is part of the list of all sub-sets of that **Set**. 

Let's take, for example, the list of all the values that can be extracted from **any given object**. How about the list of its properties? But what about the list of properties about its properties? And what about the- you get the picture. 

We don't know beforehand what type of value we want to validate. Let's say we want to validate some properties of fruit. Here is an example of some values which can be provided for fruit, and a hopefully-appropriate corresponding data-type:

```
age (days): int
color: string
purchasedOn: date
weight (g): double
family: fruitFamily
```

I added the last one to emphasise something: the values we want to validate may turn out to be some complex types. The `FruitFamily` type may be some in-house type, known only to that one software developer. Fortunately, we can write a solution which handles such cases with ease.

The generic `ValidationRule<TRules, TObject>` is too wide in scope to cover this variety. We would also like to define different rules in the same way, regardless of the type of value it is supposed to validate.

### 3.4.1 ValidationRule&lt;TRules, TObject, TValue&gt;

```
class ValidationRule<TRules, TObject, TValue> : ValidationRule<TRules, TObject>
{
}
```

The `FruitFamily` example may have painted a darker picture that what is actually going on. By splitting business logic into 2 key parts.

We begin by taking into account the validated value, as defined by the **type parameter** `TValue`. Now our `ValidationRule` specifies which type of value it can validate.

Unfortunately, knowing what to validate and actually validating the thing are two different discussions. We've already handled the former, now it's time for the latter.

To figure out how a validation rule can validate some value, we need to think like a validation rule.

### 3.4.2 What to get and what to do?

Imagine you're a validation rule. Your one job is making sure that thing you're checking fits some criteria that is out of your sphere of influence and control. Imagine you're so good at it, that you just need to know what you're checking and how you should check it, and voila! You just do. And it works. Every time.

In order to be able to apply seemingly random criteria on any given value, without prior knowledge of what the value or criteria are, seems like a lot even for our prolific validation rule. 

Such versatility comes easy through the power of configuration. If we specify to our validation rule **what** to validate and **how** to validate it, we relieve it of the burden of knowledge, and allow it to focus only on the actual validation.

### 3.4.3 What to validate?

The **what** is relatively easy. Since the validated value can come from virtually any source, we make use of lambdas to specify a function which, when called, returns the value the rule is supposed to validate, like so:

```
Func<TValue> ValueProvider;
```

Since in practice, we would want to validate values derived from our **object**, we should also include the **object** in the parameters provided to this function.

```
class ValidationRule<TRules, TObject, TValue> : ValidationRule<TRules, TObject>
{
    public Func<TObject, TValue> ValueProvider { get; set; }
}
```

This way, the `ValueProvider` function can be called on the validated object.

Now the validation rule can be told **what** to validate. Because the value is returned by a function, it's easy in practice to configure different and varied ways of obtaining these values.

### 3.4.4 How to validate?

The **how** gives us a bit more to think about. We want to tell the validation rule how to validate the value yielded by the `ValueProvider` function. 

We're going to use another lambda for this, but we have to keep something in mind: some validation rules require extra parameters to be specified.
This new lambda will check that the value respects some criteria, as defined by its implementation.

```
class ValidationRule<TRules, TObject, TValue> : ValidationRule<TRules, TObject>
{
    public Func<TObject, TValue> ValueProvider { get; set; }
    public Func<TValue, RuleParameters, bool> Comparator { get; set; }
}
```

Our `Comparator` function takes as parameters the validated value, and any extra parameters. It returns a `bool` indicating whether the value passes the validation criteria as defined by the function's implementation.

The `ValidationRule` class can now be told **how** to validate the value by providing it a comparator function. Now all that needs doing is putting the **what** and the **how** together.

## 3.5 Less talking, more validating

Back when we created the abstract class `ValidationRule<TRules, TObject>`, we included a method definition for validation.

```
public abstract ValidationResult Validate(TObject obj, RuleParameters ruleParameters);
```

Now we will implement this method in the new class. The `ValidationRule<TRules, TObject, TValue>` implementation already has all the necessary pieces. If we were to care only about the end-result (validation success), we could write our `Validate` method like this:

```
public override ValidationResult Validate(TObject obj, RuleParameters ruleParameters)
{
    return Comparator()
}
```