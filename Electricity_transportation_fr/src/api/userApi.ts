import { apiClient } from './axiosClient';

export const userApi = {
    login: async (data: any) => {
        const response = await apiClient.post('/User/login', data);
        return response.data;
    },
    register: async (data: any) => {
        const response = await apiClient.post('/User/register', data);
        return response.data;
    },
    getProfile: async () => {
        const response = await apiClient.get(`/User/profile`);
        return response.data;
    },
    changePassword: async (data: any) => {
        const response = await apiClient.patch(`/User/password`, data);
        return response.data;
    },
    // Зверніть увагу: за логікою вашого бекенду, контакти прив'язані до конкретної угоди (dealId)
    updateContactInfo: async (dealId: number, data: any) => {
        const response = await apiClient.patch(`/User/deal/${dealId}/contact`, data);
        return response.data;
    },
    resetPassword: async (data: any) => {
        const response = await apiClient.post(`/User/reset-password`, data);
        return response.data;
    }
};