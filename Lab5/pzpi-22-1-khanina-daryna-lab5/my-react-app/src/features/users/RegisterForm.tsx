import {observer} from "mobx-react-lite";
import {useStore} from "../../app/stores/store.ts";
import {ErrorMessage, Formik} from "formik";
import {Button, Header, Form} from "semantic-ui-react";
import MyTextInput from "../../app/common/form/MyTextInput.tsx";
import * as Yup from "yup";
import ValidationError from "../errors/ValidationError.tsx";

export default observer(function RegisterForm() {
   const {userStore} = useStore();

   return (
      <Formik
         initialValues={{email: '', password: '', error: null}}
         onSubmit={(values, {setErrors}) =>
            userStore.register(values).catch((error) =>
               setErrors({error}))}
         validationSchema={Yup.object({
            email: Yup.string().required("Email is required"),
            password: Yup.string().required("Password is required"),
         })}
      >
         {({handleSubmit, isSubmitting, errors, isValid, dirty}) => (
            <Form className="ui from error" onSubmit={handleSubmit} autoComplete="off">
               <Header as="h2" content="Sign up to DineMetrics" color="teal" textAlign="center" />
               <MyTextInput placeholder="Email" name="email" />
               <MyTextInput placeholder="Password" name="password" type="password" />
               <ErrorMessage
                  name="error" render={() => <ValidationError errors={errors.error as unknown as string[]} />}
               />
               <Button
                  disabled={!isValid || !dirty || isSubmitting}
                  loading={isSubmitting} positive
                  content="Register" type="submit"
                  fluid />
            </Form>
         )}
      </Formik>
   )
})