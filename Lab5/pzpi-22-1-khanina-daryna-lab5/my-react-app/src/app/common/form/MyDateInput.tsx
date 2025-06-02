import { useField } from "formik";
import { Form, Label } from "semantic-ui-react";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";

interface MyDateInputProps {
   name: string;
   placeholder?: string;
   dateFormat?: string;
   showYearDropdown?: boolean;
   showMonthDropdown?: boolean;
   isClearable?: boolean;
}

export default function MyDateInput({
                                       name,
                                       placeholder,
                                       dateFormat = "yyyy-MM-dd",
                                       showYearDropdown = true,
                                       showMonthDropdown = true,
                                       isClearable = false,
                                    }: MyDateInputProps) {
   const [field, meta, helpers] = useField(name);

   return (
      <Form.Field error={meta.touched && !!meta.error}>
         <DatePicker
            selected={field.value ? new Date(field.value) : null}
            onChange={(date: Date | null) =>
               helpers.setValue(date ? date.toISOString().split("T")[0] : "")
            }
            onBlur={() => helpers.setTouched(true)}
            placeholderText={placeholder}
            dateFormat={dateFormat}
            showYearDropdown={showYearDropdown}
            showMonthDropdown={showMonthDropdown}
            isClearable={isClearable}
         />
         {meta.touched && meta.error && (
            <Label basic color="red" content={meta.error} />
         )}
      </Form.Field>
   );
}
