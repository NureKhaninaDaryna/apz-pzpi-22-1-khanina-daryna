import {Container, Dropdown, Icon, Menu} from "semantic-ui-react";
import {NavLink} from "react-router-dom";
import {observer} from "mobx-react-lite";
import {useStore} from "../stores/store.ts";
import {hasAccess, ManagementName, PermissionAccess} from "../stores/permissionStore.ts";

export default observer(function NavBar() {
   const { userStore } = useStore();
   const { user, logout } = userStore;

   return (
      <Menu inverted fixed="top">
         <Container>
            <Menu.Item as={NavLink} to="/" header>
               <img src="/assets/icon-eatery.png" alt="logo" style={{ marginRight: '10px' }} />
               DineMetrics
            </Menu.Item>

            {user && hasAccess(user.role, ManagementName.EateriesManagement, PermissionAccess.Read) && (
               <Menu.Item as={NavLink} to="/eateries" name="Eateries" />
            )}

            {user && hasAccess(user.role, ManagementName.UsersManagement, PermissionAccess.Full) && (
               <Menu.Item as={NavLink} to="/users" name="Users" />
            )}

            {user && hasAccess(user.role, ManagementName.AnalyticsManagement, PermissionAccess.Read) && (
               <Dropdown item text="Analytics">
                  <Dropdown.Menu>
                     <Dropdown.Item as={NavLink} to="/dashboard">
                        <Icon name="chart bar" /> Dashboard
                     </Dropdown.Item>
                     <Dropdown.Item as={NavLink} to="/trends">
                        <Icon name="line graph" /> Trends
                     </Dropdown.Item>
                     <Dropdown.Item as={NavLink} to="/report">
                        <Icon name="file alternate" /> Report
                     </Dropdown.Item>
                  </Dropdown.Menu>
               </Dropdown>
            )}

            {user && (
               <Menu.Menu position="right">
                  <Dropdown item text={user.email} pointing="top right" style={{ color: 'white' }}>
                     <Dropdown.Menu>
                        <Dropdown.Item as={NavLink} to="/change-password">
                           Change Password
                        </Dropdown.Item>
                        <Dropdown.Item onClick={logout}>
                           Logout
                        </Dropdown.Item>
                     </Dropdown.Menu>
                  </Dropdown>
               </Menu.Menu>
            )}
         </Container>
      </Menu>
   );
})