import apiClient from "./ApiService";

export async function login(username, password) {
    const result = await apiClient.post('/api/auth/login', {
        username, password
    });

    return result.data;
}