import {observer} from "mobx-react-lite";
import {useStore} from "../../app/stores/store.ts";
import {ErrorMessage, Formik} from "formik";
import {Button, Header, Label, Form} from "semantic-ui-react";
import MyTextInput from "../../app/common/form/MyTextInput.tsx";

export default observer(function LoginForm() {
   const {userStore} = useStore();

   return (
      <Formik
         initialValues={{email: "", password: "", error: null}}
         onSubmit={(values, {setErrors}) =>
            userStore.login(values).catch(() => setErrors({error: 'Invalid email or password'}))}>
         {({handleSubmit, isSubmitting, errors}) => (
            <Form className="ui from" onSubmit={handleSubmit} autoComplete="off">
               <Header as="h2" content="Login to DineMetrics" color="teal" textAlign="center" />
               <MyTextInput placeholder="Email" name="email" />
               <MyTextInput placeholder="Password" name="password" type="password" />
               <ErrorMessage
                  name="error" render={() => <Label style={{marginBottom: 10}} basic color="red" content={errors.error}/>}
               />
               <Button loading={isSubmitting} positive content="Login" type="submit" fluid />
            </Form>
         )}
      </Formik>
   )
})