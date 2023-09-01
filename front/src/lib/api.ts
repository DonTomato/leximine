const apiUrl = 'http://localhost:5000/';

export function get<T>(api: string) {
    return fetch(apiUrl + api)
        .then(response => response.json() as Promise<T>)
        .then(data => data as T);
}

export function post<T>(api: string, body: any) {
    return fetch(apiUrl + api, {
        method: 'POST',
        body: JSON.stringify(body),
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.json() as Promise<T>)
        .then(data => data as T);
}

export function put<T>(api: string, body: any) {
    return fetch(apiUrl + api, {
        method: 'PUT',
        body: JSON.stringify(body),
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.json() as Promise<T>)
        .then(data => data as T);
}

export function del<T>(api: string) {
    return fetch(apiUrl + api, {
        method: 'DELETE'
    })
        .then(response => response.json() as Promise<T>)
        .then(data => data as T);
}
