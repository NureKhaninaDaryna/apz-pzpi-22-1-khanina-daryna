import {observer} from "mobx-react-lite";
import {useStore} from "../../../app/stores/store.ts";
import {useEffect} from "react";
import LoadingComponent from "../../../app/layout/LoadingComponent.tsx";
import EateryList from "./EateryList.tsx";
import {Button, Container, Header, Icon} from "semantic-ui-react";
import {Link} from "react-router-dom";
import {hasAccess, ManagementName, PermissionAccess} from "../../../app/stores/permissionStore.ts";

export default observer(function EateryDashboard() {
   const { eateryStore, userStore} = useStore();
   const { user } = userStore;
   const { loadEateries, loadingInitial, eateryRegistry } = eateryStore;

   useEffect(() => {
      if (eateryRegistry.size <= 1) loadEateries();
   }, [eateryRegistry.size, loadEateries]);

   if (loadingInitial) return <LoadingComponent content="Loading eateries" />

   return (
      <Container>
         <Header as="h2" color="teal" textAlign="center">
            <Icon name="food" /> Eatery List
         </Header>

         {
            user && hasAccess(user.role, ManagementName.AnalyticsManagement, PermissionAccess.Full) && (
               <Button
                  as={Link}
                  to="/eateries/create"
                  icon
                  labelPosition="left"
                  color="green"
                  style={{ marginBottom: "20px" }}
               >
                  <Icon name="plus" />
                  Create Eatery
               </Button>
            )
         }

         <EateryList />
      </Container>
   )
})