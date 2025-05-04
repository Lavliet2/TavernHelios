export function isHttpResponseSuccess(httpCode: number) : boolean {
    return httpCode >= 200 && httpCode < 300
}