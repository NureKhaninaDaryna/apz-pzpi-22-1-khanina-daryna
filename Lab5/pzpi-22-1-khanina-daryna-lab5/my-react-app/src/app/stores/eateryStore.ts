import {makeAutoObservable, runInAction} from "mobx";
import {Eatery} from "../models/eatery.ts";
import agent from "../api/agent.ts";

export default class EateryStore {
   eateryRegistry = new Map<string, Eatery>();
   eateries: Eatery[] = [];
   selectedEatery: Eatery | undefined = undefined;
   editMode = false;
   loading = false;
   loadingInitial = false;

   constructor() {
      makeAutoObservable(this);
   }

   loadEateries = async () => {
      this.setLoadingInitial(true);
      try {
         const eateries = await agent.Eateries.list();
         runInAction(() => {
            this.eateries = eateries;
            eateries.forEach(eatery => {
               this.setEatery(eatery);
            });
            this.setLoadingInitial(false);
         })
      } catch (error) {
         console.log(error);
         this.setLoadingInitial(false);
      }
   }

   loadEatery = async (eateryId: number) => {
      this.setLoadingInitial(true);

      let eatery = this.getEateryById(eateryId);
      if (eatery) {
         runInAction(() => {
            this.selectedEatery = eatery;
            this.loadingInitial = false;
         })
         return eatery;
      } else {
         try {
            eatery = await agent.Eateries.details(eateryId);
            if (eatery) this.setEatery(eatery);
            runInAction(() => {
               this.selectedEatery = eatery;
               this.loadingInitial = false;
            })
            return eatery;
         } catch (error) {
            console.log(error);
            runInAction(() => {
               this.loadingInitial = false;
            })
         }
      }
   }

   createEatery = async (eatery: Eatery) => {
      this.loading = true;
      try {
         const id = await agent.Eateries.create(eatery);
         eatery.id = id;

         runInAction(() => {
            this.eateryRegistry.set(eatery.id.toString(), eatery);
            this.selectedEatery = eatery;
            this.editMode = false;
            this.loading = false;
         })
      } catch (error) {
         console.log(error);
         runInAction(() => {
            this.loading = false;
         })
      }
   }

   updateEatery = async (eatery: Eatery) => {
      this.loading = true;
      try {
         await agent.Eateries.update(eatery);

         runInAction(() => {
            this.eateryRegistry.set(eatery.id.toString(), eatery);
            this.selectedEatery = eatery;
            this.editMode = false;
            this.loading = false;
         })
      } catch (error) {
         console.log(error);
         runInAction(() => {
            this.loading = false;
         })
      }
   }

   deleteEatery = async (id: number) => {
      this.loading = true;
      try {
         await agent.Eateries.delete(id);

         runInAction(() => {
            this.eateryRegistry.delete(id.toString());
            this.loading = false;
         })
      } catch (error) {
         console.log(error);
         runInAction(() => {
            this.loading = false;
         })
      }
   }

   setLoadingInitial = (state: boolean) => {
      this.loadingInitial = state;
   }

   private setEatery = (eatery: Eatery) => {
      this.eateryRegistry.set(eatery.id.toString(), eatery);
   }

   private getEateryById = (id: number) => {
      return this.eateryRegistry.get(id.toString());
   }
}