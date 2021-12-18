const requestOptions = (method, data) => {
    return { 
        method: method, 
        body: data ? JSON.stringify(data) : null,
        headers: {
            'Content-Type': 'application/json'
        }
    }
}

const queryParams = data => new URLSearchParams(data).toString();

export default {
    get: async (url, data) => await fetch(`${url}?${queryParams(data)}`, requestOptions('GET')).then(response => response.json()),
    post: async (url, data) => await fetch(url, requestOptions('POST', data)).then(response => response.json())
}