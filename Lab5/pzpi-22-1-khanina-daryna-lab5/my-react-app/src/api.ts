import axios from "axios";

const API_BASE_URL = "http://localhost:5048";

const api = axios.create({
   baseURL: API_BASE_URL,
   headers: {
      "Content-Type": "application/json",
   },
});

export interface ApiResponse<T> {
   success: boolean;
   data: T;
   message?: string;
}

export const fetchData = async <T>(endpoint: string): Promise<ApiResponse<T>> => {
   try {
      const response = await api.get<T>(endpoint);
      return { success: true, data: response.data };
   } catch (error) {
      console.error("Error:", error);
      return { success: false, data: {} as T, message: "Ошибка запроса" };
   }
};

export default api;
