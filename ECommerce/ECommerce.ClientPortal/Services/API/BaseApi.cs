using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Json;

namespace ECommerce.ClientPortal.Services.API;

public abstract class BaseApi
{
    protected readonly HttpClient _http;
    protected readonly NavigationManager _navigation;

    protected BaseApi(HttpClient http, NavigationManager navigation)
    {
        _http = http;
        _navigation = navigation;
    }

    protected async Task<T?> SafeGet<T>(string url)
    {
        try
        {
            return await _http.GetFromJsonAsync<T>(url);
        }
        catch (HttpRequestException ex)
        {
            HandleHttpError(ex.StatusCode);
            return default;
        }
        catch
        {
            _navigation.NavigateTo("/error", true);
            return default;
        }
    }

    protected async Task<List<T>> SafeGetList<T>(string url)
    {
        try
        {
            var result = await _http.GetFromJsonAsync<List<T>>(url);
            return result ?? new List<T>();
        }
        catch (HttpRequestException ex)
        {
            HandleHttpError(ex.StatusCode);
            return new List<T>();
        }
        catch
        {
            _navigation.NavigateTo("/error", true);
            return new List<T>();
        }
    }

    protected async Task<T?> SafePost<T>(string url, object? body = null)
    {
        try
        {
            var response = await _http.PostAsJsonAsync(url, body);
            if (!response.IsSuccessStatusCode)
            {
                HandleHttpError(response.StatusCode);
                return default;
            }

            return typeof(T) switch
            {
                Type t when t == typeof(string)
                    => (T)(object)await response.Content.ReadAsStringAsync(),

                Type t when t == typeof(bool)
                    => (T)(object)true,

                Type t when t == typeof(void) || t == typeof(object)
                    => default,

                _ => await response.Content.ReadFromJsonAsync<T>()
            };
        }
        catch (HttpRequestException ex)
        {
            HandleHttpError(ex.StatusCode);
            return default;
        }
        catch
        {
            _navigation.NavigateTo("/error", true);
            return default;
        }
    }

    protected async Task<string?> SafePostMultipart(string url, MultipartFormDataContent content)
    {
        try
        {
            var response = await _http.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                HandleHttpError(response.StatusCode);
                return null;
            }

            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException ex)
        {
            HandleHttpError(ex.StatusCode);
            return null;
        }
        catch
        {
            _navigation.NavigateTo("/error", true);
            return null;
        }
    }

    protected async Task<bool> SafePut(string url, object? body = null)
    {
        try
        {
            var response = await _http.PutAsJsonAsync(url, body);
            if (!response.IsSuccessStatusCode)
                HandleHttpError(response.StatusCode);

            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            HandleHttpError(ex.StatusCode);
            return false;
        }
        catch
        {
            _navigation.NavigateTo("/error", true);
            return false;
        }
    }

    protected async Task<bool> SafeDelete(string url)
    {
        try
        {
            var response = await _http.DeleteAsync(url);
            if (!response.IsSuccessStatusCode)
                HandleHttpError(response.StatusCode);

            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            HandleHttpError(ex.StatusCode);
            return false;
        }
        catch
        {
            _navigation.NavigateTo("/error", true);
            return false;
        }
    }

    private void HandleHttpError(HttpStatusCode? status)
    {
        switch (status)
        {
            case HttpStatusCode.Unauthorized:
                _navigation.NavigateTo("/Auth/Login", true);
                break;

            case HttpStatusCode.Forbidden:
                _navigation.NavigateTo("/access-denied", true);
                break;

            case HttpStatusCode.NotFound:
                _navigation.NavigateTo("/404", true);
                break;

            default:
                _navigation.NavigateTo("/error", true);
                break;
        }
    }
}