import {createContext, useContext} from "react";
import CommonStore from "./commonStore.ts";
import UserStore from "./userStore.ts";
import ModalStore from "../models/modalStore.ts";
import EateryStore from "./eateryStore.ts";
import AnalyticsStore from "./analyticsStore.ts";

interface Store {
   commonStore: CommonStore;
   userStore: UserStore;
   modalStore: ModalStore;
   eateryStore: EateryStore;
   analyticsStore: AnalyticsStore;
}

export const store: Store = {
   commonStore: new CommonStore(),
   userStore: new UserStore(),
   modalStore: new ModalStore(),
   eateryStore: new EateryStore(),
   analyticsStore: new AnalyticsStore(),
}

export const StoreContext = createContext<Store>(store);

export function useStore() {
   return useContext(StoreContext);
}