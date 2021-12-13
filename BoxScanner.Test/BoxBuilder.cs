using System;
using BoxFilterExample;

namespace BoxFilter.Test
{
    public class BoxBuilder
    {
        private static Random Rng = new();
        private static int Roll => Rng.Next(0, 100);

        private int _height = Rng.Next(101, 150);
        private int _width = Rng.Next(101, 150);
        private int _depth = Rng.Next(101, 150);
        private int _weight = Rng.Next(1001, 5000);
        private int _age = Rng.Next(2, 5);
        private string _color = "Brown";

        public static Box RandomBox()
        {
            var boxBuilder = new BoxBuilder();

            if (Roll <= 25) boxBuilder = Roll <= 50 ? boxBuilder.Thick() : boxBuilder.Thin();
            if (Roll <= 25) boxBuilder = Roll <= 50 ? boxBuilder.Tall() : boxBuilder.Short();
            if (Roll <= 25) boxBuilder = Roll <= 50 ? boxBuilder.Deep() : boxBuilder.Shallow();
            if (Roll <= 25) boxBuilder = Roll <= 50 ? boxBuilder.Heavy() : boxBuilder.Light();
            if (Roll <= 25) boxBuilder = Roll <= 50 ? boxBuilder.New() : boxBuilder.Old();
            if (Roll <= 25) boxBuilder = Roll <= 50 ? boxBuilder.Colored("Black") : boxBuilder.Colored("White");

            return boxBuilder.Build();
        }

        public Box Build()
        {
            return new Box
            {
                Height = _height,
                Weight = _weight,
                Width = _width,
                Depth = _depth,
                Color = _color,
                ReceivedOn = DateTime.UtcNow.AddDays(-_age)
            };
        }

        public BoxBuilder Short()
        {
            _height = Rng.Next(1, 100);
            return this;
        }

        public BoxBuilder Tall()
        {
            _height = Rng.Next(151, 300);
            return this;
        }
        public BoxBuilder Thin()
        {
            _width = Rng.Next(1, 100);
            return this;
        }
        public BoxBuilder Thick()
        {
            _width = Rng.Next(151, 300);
            return this;
        }
        public BoxBuilder Shallow()
        {
            _depth = Rng.Next(1, 100);
            return this;
        }
        public BoxBuilder Deep()
        {
            _depth = Rng.Next(151, 300);
            return this;
        }
        public BoxBuilder Light()
        {
            _weight = Rng.Next(1, 1000);
            return this;
        }
        public BoxBuilder Heavy()
        {
            _weight = Rng.Next(5001, 20000);
            return this;
        }

        public BoxBuilder Colored(string color)
        {
            _color = color;
            return this;
        }

        public BoxBuilder New()
        {
            _age = Rng.Next(0, 1);
            return this;
        }
        public BoxBuilder Old()
        {
            _age = Rng.Next(3, 7);
            return this;
        }
    }
}
