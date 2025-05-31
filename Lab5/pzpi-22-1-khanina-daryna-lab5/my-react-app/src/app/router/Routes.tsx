import App from "../layout/App.tsx";
import {createBrowserRouter, Navigate, RouteObject} from "react-router-dom";
import NotFound from "../../features/errors/NotFound.tsx";
import ServerError from "../../features/errors/ServerError.tsx";
import HomePage from "../../features/home/HomePage.tsx";
import EateryDashboard from "../../features/eateries/dashboard/EateryDashboard.tsx";
import EateryForm from "../../features/eateries/forms/EateryForm.tsx";
import DashboardMetrics from "../../features/analytics/DashboardMetrics.tsx";
import TrendChart from "../../features/analytics/TrendChart.tsx";
import ReportDownloadPage from "../../features/analytics/ReportDownloadPage.tsx";
import ChangePasswordForm from "../../features/users/ChangePasswordForm.tsx";
import UserListPage from "../../features/users/UserListPage.tsx";

export const routes: RouteObject[] = [
   {
      path: '/',
      element: <App />,
      children: [
         { path: '', element: <HomePage />},
         { path: "eateries", element: <EateryDashboard /> },
         { path: "eateries/create", element: <EateryForm key="create" /> },
         { path: "eateries/:id/edit", element: <EateryForm key="manage" /> },
         { path: 'not-found', element: <NotFound />},
         { path: 'server-error', element: <ServerError />},
         { path: 'dashboard', element: <DashboardMetrics />},
         { path: 'trends', element: <TrendChart />},
         { path: 'report', element: <ReportDownloadPage />},
         { path: 'change-password', element: <ChangePasswordForm />},
         { path: 'users', element: <UserListPage />},
         { path: '*', element: <Navigate replace to="/not-found" />},
      ]
   },
]

export const router = createBrowserRouter(routes);