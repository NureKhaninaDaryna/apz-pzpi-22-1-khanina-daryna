import { createRoot } from 'react-dom/client'
import {store, StoreContext} from "./app/stores/store.ts";
import {RouterProvider} from "react-router-dom";
import {router} from "./app/router/Routes.tsx";
import 'react-calendar/dist/Calendar.css';
import 'react-datepicker/dist/react-datepicker.css';
import 'semantic-ui-css/semantic.min.css'
import 'react-toastify/dist/ReactToastify.css'
import './app/layout/styles.css'

createRoot(document.getElementById('root')!).render(
   <StoreContext.Provider value={store}>
      <RouterProvider router={router} />
   </StoreContext.Provider>
)
