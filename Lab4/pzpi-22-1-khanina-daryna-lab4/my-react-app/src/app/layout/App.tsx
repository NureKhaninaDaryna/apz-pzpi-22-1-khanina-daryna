import {useEffect} from 'react'
import {Outlet, useLocation} from "react-router-dom";
import {useStore} from "../stores/store.ts";
import LoadingComponent from "./LoadingComponent.tsx";
import {toast, ToastContainer} from "react-toastify";
import {Container} from "semantic-ui-react";
import HomePage from "../../features/home/HomePage.tsx";
import NavBar from "./NavBar.tsx";
import ModalContainer from "../common/modals/ModalContainer.tsx";
import {observer} from "mobx-react-lite";
import {startSignalR} from "../services/notificationService.ts";

const App = observer(function App() {
   const location = useLocation();
   const { commonStore, userStore } = useStore();

   useEffect(() => {
      if (commonStore.token) {
         userStore.getUser().finally(() => commonStore.setAppLoaded());
      } else {
         commonStore.setAppLoaded();
      }
   }, [commonStore.token]);

   useEffect(() => {
      startSignalR((title, message) => {
         toast.info(
            <div style={{ maxWidth: '350px', wordBreak: 'break-word' }}>
               <strong>{title}</strong>
               <div style={{ marginTop: '5px' }}>{message}</div>
            </div>
         );
      });
   }, []);

   if (!commonStore.appLoaded) return <LoadingComponent content="Loading App" />;

   return (
      <>
         <ModalContainer />
         <ToastContainer position="bottom-right" hideProgressBar theme="colored" autoClose={5000} />
         {location.pathname === "/" ? <HomePage /> : (
            <>
               <NavBar />
               <Container style={{ paddingTop: '7em' }}>
                  <Outlet />
               </Container>
            </>
         )}
      </>
   );
});

export default App;
