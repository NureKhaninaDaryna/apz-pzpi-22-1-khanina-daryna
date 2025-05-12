import { useState } from 'react';
import { Button, Dropdown, Form } from 'semantic-ui-react';
import { observer } from 'mobx-react-lite';
import { useStore } from '../../app/stores/store';
import { UserWithId, UserRole } from '../../app/models/user.ts';

interface Props {
   user: UserWithId;
}

const getRoleOptions = (excludeRole: UserRole) => {
   return Object.keys(UserRole)
      .filter(key => isNaN(Number(key)))
      .map(key => ({
         key,
         text: key.replace(/([A-Z])/g, ' $1').trim(),
         value: key
      }))
      .filter(option => option.value !== UserRole[excludeRole]);
};

export default observer(function RoleUpdateModal({ user }: Props) {
   const { userStore, modalStore } = useStore();
   const [selectedRole, setSelectedRole] = useState(user.role);
   const [loading, setLoading] = useState(false);

   const roleOptions = getRoleOptions(user.role);

   const handleUpdateRole = async () => {
      setLoading(true);
      try {
         await userStore.updateUserRole(user.id, selectedRole);
         modalStore.closeModal();
      } catch (err) {
         console.error(err);
      } finally {
         setLoading(false);
      }
   };

   return (
      <Form loading={loading}>
         <Form.Field>
            <label>Email</label>
            <input value={user.email} disabled />
         </Form.Field>
         <Form.Field>
            <label>Role</label>
            <Dropdown
               fluid
               selection
               options={roleOptions}
               value={selectedRole}
               onChange={(_, { value }) => setSelectedRole(value as UserRole)}
            />
         </Form.Field>
         <Button onClick={handleUpdateRole} primary fluid disabled={!selectedRole}>
            Save
         </Button>
      </Form>
   );
});
