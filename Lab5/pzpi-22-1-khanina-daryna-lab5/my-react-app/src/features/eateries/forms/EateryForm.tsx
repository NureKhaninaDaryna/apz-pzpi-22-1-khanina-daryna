import {observer} from "mobx-react-lite";
import {useStore} from "../../../app/stores/store.ts";
import {Link, useNavigate, useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import {Eatery, EateryType} from "../../../app/models/eatery.ts";
import * as Yup from "yup";
import LoadingComponent from "../../../app/layout/LoadingComponent.tsx";
import { Button, Segment, Header } from "semantic-ui-react";
import { Formik, Form as FormikForm, } from "formik";
import MyTextInput from "../../../app/common/form/MyTextInput.tsx";
import MyDateInput from "../../../app/common/form/MyDateInput.tsx";
import MySelectInput from "../../../app/common/form/MySelectInput.tsx";

export default observer(function EateryForm() {
   const { eateryStore } = useStore();
   const { createEatery, updateEatery,
      loading, loadEatery, loadingInitial } = eateryStore;
   const { id } = useParams();
   const navigate = useNavigate();

   const eateryTypeOptions = Object.entries(EateryType)
      .filter(([, value]) => !isNaN(Number(value)))
      .map(([key, value]) => ({
         text: key,
         value: Number(value),
      }));

   const [eatery, setEatery] = useState<Eatery>({
      id: 0,
      name: "",
      address: "",
      type: EateryType.Restaurant,
      openingDay: "",
      operatingHours: "",
      maximumCapacity: 0,
      temperatureThreshold: 0,
   });

   const validationSchema = Yup.object({
      name: Yup.string()
         .min(3, "Name must be at least 3 characters")
         .max(50, "Name must be at most 50 characters")
         .required("Name is required"),

      address: Yup.string()
         .min(5, "Address must be at least 5 characters")
         .max(100, "Address must be at most 100 characters")
         .required("Address is required"),

      type: Yup.number()
         .oneOf([0, 1, 2, 3, 4, 5], "Invalid eatery type")
         .required("Eatery type is required"),

      openingDay: Yup.date()
         .typeError("Invalid date format")
         .required("Opening day is required"),

      operatingHours: Yup.string()
         .matches(
            /^([01]\d|2[0-3]):([0-5]\d)-([01]\d|2[0-3]):([0-5]\d)$/,
            "Operating hours must be in format HH:mm-HH:mm"
         )
         .required("Operating hours are required"),

      maximumCapacity: Yup.number()
         .min(1, "Capacity must be at least 1")
         .max(1000, "Capacity cannot exceed 1000")
         .required("Maximum capacity is required"),

      temperatureThreshold: Yup.number()
         .min(-50, "Temperature cannot be lower than -50°C")
         .max(50, "Temperature cannot be higher than 50°C")
         .required("Temperature threshold is required"),
   });

   useEffect(() => {
      if (id) loadEatery(+id).then(eatery => setEatery(eatery!));
   }, [id, loadEatery]);

   function handleFormSubmit(eatery: Eatery) {
      if (!eatery.id) {
         createEatery(eatery).then(() => {
            navigate(`/eateries`);
         });
      } else {
         updateEatery(eatery).then(() => {
            navigate(`/eateries`);
         })
      }
   }

   if (loadingInitial) return <LoadingComponent content="Loading..." />

   return (
      <>
         <Segment clearing>
            <Header as="h2" content={id ? "Edit Eatery" : "Create Eatery"} color="teal" textAlign="center" />
            <Formik
               initialValues={eatery}
               validationSchema={validationSchema}
               enableReinitialize
               onSubmit={handleFormSubmit}
            >
               {({ handleSubmit, isValid, dirty, isSubmitting }) => (
                  <FormikForm className="ui form" onSubmit={handleSubmit}>
                     <MyTextInput placeholder="Name" name="name" />
                     <MyTextInput placeholder="Address" name="address" />
                     <MySelectInput
                        name="type"
                        placeholder="Choose type"
                        options={eateryTypeOptions}
                     />
                     <MyDateInput
                        placeholderText='Date'
                        name='openingDay'
                        dateFormat="yyyy-MM-dd"
                     />
                     <MyTextInput placeholder="e.g. 08:00-22:00" name="operatingHours" />
                     <MyTextInput placeholder="Maximum Capacity" name="maximumCapacity" type="number" />
                     <MyTextInput placeholder="Enter temperature threshold" name="temperatureThreshold" type="number" />
                     <Button
                        disabled={isSubmitting || !isValid || !dirty}
                        loading={loading}
                        positive
                        type="submit"
                        content="Submit"
                        floated="right"
                     />
                     <Button as={Link} to="/eateries" type="button" content="Cancel" floated="right"/>
                  </FormikForm>
               )}
            </Formik>
         </Segment>
      </>
   )
});