const apiUrl = 'http://localhost:5000/';

export function get<T>(api: string) {
    return fetch(apiUrl + api)
        .then(response => response.json() as Promise<T>)
        .then(data => data as T);
}
