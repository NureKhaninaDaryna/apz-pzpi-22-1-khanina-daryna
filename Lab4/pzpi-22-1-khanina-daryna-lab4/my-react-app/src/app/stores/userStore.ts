import {User, UserFromValues, UserRole, UserWithId} from "../models/user.ts";
import {makeAutoObservable, runInAction} from "mobx";
import agent from "../api/agent.ts";
import {store} from "./store.ts";
import {router} from "../router/Routes.tsx";

export default class UserStore {
   user: User | null = null;
   users: UserWithId[] = [];
   loading: boolean = false;

   constructor() {
      makeAutoObservable(this);
   }

   get isLoggedIn(): boolean {
      return !!this.user;
   }

   loadUsers = async () => {
      if (this.users.length === 0) {
         this.loading = true;
         const users = await agent.Account.getAll();
         runInAction(() => {
            this.users = users;
            this.loading = false;
         });
      }
   };

   login = async (creds: UserFromValues): Promise<void> => {
      try {
         const user = await agent.Account.login(creds);
         store.commonStore.setToken(user.token);
         runInAction(() => this.user = user);
         await router.navigate('/eateries');
         store.modalStore.closeModal()
      } catch (error) {
         console.log(error);
         throw error;
      }
   }

   register = async (creds: UserFromValues): Promise<void> => {
      try {
         const user = await agent.Account.register(creds);
         store.commonStore.setToken(user.token);
         runInAction(() => this.user = user);
         store.modalStore.closeModal();
      } catch (error) {
         console.log(error);
         throw error;
      }
   }

   logout = () => {
      store.commonStore.setToken(null);
      runInAction(() => this.user = null);
      router.navigate('/');
   }

   changePassword = async (current: string, newPass: string) => {
      try {
         await agent.Account.changePassword({ currentPassword: current, newPassword: newPass });
      } catch (error) {
         console.log(error);
         throw error;
      }
   };

   updateUserRole = async (id: number, role: UserRole) => {
      try {
         await agent.Account.updateRole({ userId: id, role });
         runInAction(() => {
            const userIndex = this.users.findIndex(u => u.id === id);
            if (userIndex !== -1) {
               this.users[userIndex] = { ...this.users[userIndex], role };
            }
         });
      } catch (error) {
         console.log(error);
         throw error;
      }
   };

   getUser = async () => {
      try {
         const user = await agent.Account.current();
         runInAction(() => this.user = user);
      } catch (error) {
         console.log(error);
      }
   }
}