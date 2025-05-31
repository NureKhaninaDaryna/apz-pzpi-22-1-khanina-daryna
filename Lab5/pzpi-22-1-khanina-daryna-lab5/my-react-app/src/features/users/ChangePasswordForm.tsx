import { useState } from 'react';
import { Button, Form, Header, Message, Segment } from 'semantic-ui-react';
import { observer } from 'mobx-react-lite';
import { useStore } from '../../app/stores/store.ts';

export default observer(function ChangePasswordForm() {
   const { userStore } = useStore();
   const [currentPassword, setCurrentPassword] = useState('');
   const [newPassword, setNewPassword] = useState('');
   const [confirmPassword, setConfirmPassword] = useState('');
   const [error, setError] = useState<string | null>(null);
   const [success, setSuccess] = useState(false);
   const [loading, setLoading] = useState(false);

   const handleSubmit = async () => {
      setError(null);
      setSuccess(false);

      const validationErrors = [];

      const passwordRegex = /[A-Z]/;

      if (!currentPassword || currentPassword.length < 8 || !passwordRegex.test(currentPassword)) {
         validationErrors.push("Current password must be at least 8 characters long and contain at least one uppercase letter.");
      }

      if (!newPassword || newPassword.length < 8 || !passwordRegex.test(newPassword)) {
         validationErrors.push("New password must be at least 8 characters long and contain at least one uppercase letter.");
      }

      if (newPassword === currentPassword) {
         validationErrors.push("New password must not be the same as current password.");
      }

      if (newPassword !== confirmPassword) {
         validationErrors.push("New passwords do not match.");
      }

      if (validationErrors.length > 0) {
         setError(validationErrors.join('\n'));
         return;
      }

      setLoading(true);
      try {
         await userStore.changePassword(currentPassword, newPassword);
         setSuccess(true);
         setCurrentPassword('');
         setNewPassword('');
         setConfirmPassword('');
      } catch (err: any) {
         const backendError = err?.response?.data;
         if (Array.isArray(backendError)) {
            setError(backendError.join('\n'));
         } else if (typeof backendError === 'string') {
            setError(backendError);
         } else {
            setError('Failed to change password.');
         }
      } finally {
         setLoading(false);
      }
   };

   return (
      <Segment>
         <Header as="h2" content="Change Password" />
         <Form onSubmit={handleSubmit} loading={loading} success={success} error={!!error}>
            <Form.Input
               label="Current Password"
               type="password"
               value={currentPassword}
               onChange={(e) => setCurrentPassword(e.target.value)}
               required
            />
            <Form.Input
               label="New Password"
               type="password"
               value={newPassword}
               onChange={(e) => setNewPassword(e.target.value)}
               required
            />
            <Form.Input
               label="Confirm New Password"
               type="password"
               value={confirmPassword}
               onChange={(e) => setConfirmPassword(e.target.value)}
               required
            />
            <Message success content="Password changed successfully!" />
            <Message error content={error} />
            <Button primary type="submit" fluid>
               Change Password
            </Button>
         </Form>
      </Segment>
   );
});
