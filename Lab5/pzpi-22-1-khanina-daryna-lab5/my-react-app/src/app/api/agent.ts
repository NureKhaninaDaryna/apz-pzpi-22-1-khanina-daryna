import {store} from "../stores/store.ts";
import axios, {AxiosRequestConfig} from "axios";
import type { AxiosError, AxiosResponse } from "axios";
import {toast} from "react-toastify";
import {router} from "../router/Routes.tsx";
import {User, UserDto, UserFromValues, UserWithId} from "../models/user.ts";
import {Eatery} from "../models/eatery.ts";
import {DashboardDataDto, TrendAnalysisDto} from "../models/analytics.ts";

const sleep = (delay: number) => {
   return new Promise(resolve => {
      setTimeout(resolve, delay)
   });
};

axios.defaults.baseURL = "http://localhost:8080";

axios.interceptors.request.use(config => {
   const token = store.commonStore.token;
   if (token && config.headers) config.headers.Authorization = `Bearer ${token}`
   return config;
});

axios.interceptors.response.use(async response => {
   await sleep(1000);
   return response;
}, (error: AxiosError) => {
   const {data, status, config} = error.response as AxiosResponse;
   switch (status) {
      case 400:
         if (config.method === "get" && Object.prototype.hasOwnProperty.call(data.errors, "id")) {
            router.navigate("/not-found");
         }

         if (data.errors && typeof data.errors === 'object') {
            const modelStateErrors: string[] = [];

            for (const key of Object.keys(data.errors)) {
               if (Array.isArray(data.errors[key])) {
                  modelStateErrors.push(...data.errors[key]);
               }
            }

            throw modelStateErrors;
         } else {
            toast.error(data);
         }
         break;
      case 401:
         toast.error('Unauthorized');
         break;
      case 403:
         toast.error('Forbidden');
         break;
      case 404:
         router.navigate('/not-found');
         break;
      case 500:
         store.commonStore.setServerError(data);
         router.navigate('/server-error');
         break;
   }

   return Promise.reject(error);
});

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const request = {
   get: <T> (url: string, config?: AxiosRequestConfig<any> | undefined) => axios.get<T>(url, config).then(responseBody),
   post: <T> (url: string, body: {}, config?: AxiosRequestConfig<any> | undefined) => axios.post<T>(url, body, config).then(responseBody),
   put: <T> (url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
   del: <T> (url: string) => axios.delete<T>(url).then(responseBody),
   patch: <T> (url: string, body: {}) => axios.patch<T>(url, body).then(responseBody),
}

const Eateries = {
   list: () => request.get<Eatery[]>("/eateries"),
   details: (id: number) => request.get<Eatery>(`/eateries/${id}`),
   create: (eatery: Eatery) => request.post<number>(`/eateries`, eatery),
   update: (eatery: Eatery) => request.put<void>(`/eateries/${eatery.id}`, eatery),
   delete: (id: number) => request.del<void>(`/eateries/${id}`)
}

const Account = {
   current: () => request.get<User>("/users"),
   login: (user: UserFromValues) => request.post<User>("/users/login", user),
   register: (user: UserFromValues) => request.post<User>("/users/register", user),
   changePassword: (data: { currentPassword: string; newPassword: string }) => request.post("/users/change-password", data),
   getAll: () => request.get<UserWithId[]>("/users/all"),
   updateRole: (data: { userId: number, role: number })=> request.patch<UserDto>("/users/update-role", data)
}

const Analytics = {
   dashboardMetrics: (from?: Date, to?: Date) => request.get<DashboardDataDto>("/analytics/dashboard-metrics", {
      params: {
         from: from?.toISOString(),
         to: to?.toISOString(),
      },
   }),
   generateTrends: (facilityId: number) => request.get<TrendAnalysisDto[]>(`/analytics/trends/${facilityId}`),
   downloadReport: (start?: string, end?: string) => request.post<Blob>("/reports/download", { StartDate: start, EndDate: end }, { responseType: 'blob' }),
}

const agent = {
   Account,
   Eateries,
   Analytics
}

export default agent;