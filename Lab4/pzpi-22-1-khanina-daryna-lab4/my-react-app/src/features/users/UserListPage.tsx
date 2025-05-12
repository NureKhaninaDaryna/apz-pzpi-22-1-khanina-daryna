import { useEffect } from 'react';
import { Button, Header, Segment, Table, Loader } from 'semantic-ui-react';
import { observer } from 'mobx-react-lite';
import { useStore } from '../../app/stores/store';
import RoleUpdateModal from "./RoleUpdateModal.tsx";

export default observer(function UserListPage() {
   const { userStore, modalStore } = useStore();

   useEffect(() => {
      userStore.loadUsers();
   }, [userStore]);

   return (
      <Segment>
         <Header as="h2" content="User Management" />

         {userStore.loading ? (
            <Loader active inline="centered" />
         ) : (
            <Table celled>
               <Table.Header>
                  <Table.Row>
                     <Table.HeaderCell>Email</Table.HeaderCell>
                     <Table.HeaderCell>Role</Table.HeaderCell>
                     <Table.HeaderCell>Actions</Table.HeaderCell>
                  </Table.Row>
               </Table.Header>
               <Table.Body>
                  {userStore.users.map((user) => (
                     <Table.Row key={user.id}>
                        <Table.Cell>{user.email}</Table.Cell>
                        <Table.Cell>{user.role}</Table.Cell>
                        <Table.Cell>
                           <Button
                              content="Change Role"
                              onClick={() => modalStore.openModal(<RoleUpdateModal user={user} />)}
                           />
                        </Table.Cell>
                     </Table.Row>
                  ))}
               </Table.Body>
            </Table>
         )}
      </Segment>
   );
});
