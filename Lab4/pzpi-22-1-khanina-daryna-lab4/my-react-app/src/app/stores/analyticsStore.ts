import {makeAutoObservable, runInAction} from "mobx";
import agent from "../api/agent.ts";
import {DashboardDataDto, TrendAnalysisDto} from "../models/analytics.ts";

export default class AnalyticsStore {
   dashboardInfo: DashboardDataDto | undefined = undefined;
   fromDate: Date | undefined = undefined;
   toDate: Date | undefined = undefined;
   startDate: Date | undefined = undefined;
   endDate: Date | undefined = undefined;
   trends: TrendAnalysisDto[] | undefined = undefined;
   facilityId: number | undefined = undefined;
   loading = false;
   loadingInitial = false;

   constructor() {
      makeAutoObservable(this);
   }

   loadDashboard = async (from?: Date, to?: Date) => {
      this.setLoadingInitial(true);
      try {
         const dashboardData = await agent.Analytics.dashboardMetrics(from, to);
         runInAction(() => {
            this.dashboardInfo = dashboardData;
            this.toDate = to;
            this.fromDate = from;
            this.loadingInitial = false;
         })
         return dashboardData;
      } catch (error) {
         console.log(error);
         runInAction(() => {
            this.loadingInitial = false;
         })
      }
   }

   loadTrendsAnalysis = async (facilityId: number) => {
      this.setLoadingInitial(true);
      try {
         const trends = await agent.Analytics.generateTrends(facilityId);
         runInAction(() => {
            this.trends = trends;
            this.facilityId = facilityId;
            this.loadingInitial = false;
         })
      } catch (error) {
         console.log(error);
         runInAction(() => {
            this.loadingInitial = false;
         })
      }
   }

   downloadReport = async (startDate?: Date, endDate?: Date) => {
      this.setLoadingInitial(true);
      try {
         const startDateString = startDate?.toISOString().split('T')[0];
         const endDateString = endDate?.toISOString().split('T')[0];

         const report = await agent.Analytics.downloadReport(startDateString, endDateString);
         runInAction(() => {
            this.endDate = endDate;
            this.startDate = startDate;
            this.loadingInitial = false;
         })
         return report;
      } catch (error) {
         console.log(error);
         runInAction(() => {
            this.loadingInitial = false;
         })
      }
   }

   setLoadingInitial = (state: boolean) => {
      this.loadingInitial = state;
   }
}