import React from "react";
import { useField, useFormikContext } from "formik";
import { KeyboardDatePicker } from "@material-ui/pickers";
import { parseISO, format } from "date-fns";

function KeyboardDatePickerFormik({ ...props }) {
  const { setFieldValue } = useFormikContext();
  const [field] = useField(props);
  return (
    <KeyboardDatePicker
      {...field}
      {...props}
      size="small"
      selected={(field.value && new Date(field.value)) || null}
      onChange={(val) => {
        setFieldValue(field.name, val ? format(val, 'yyyy-MM-dd') : null);
      }}
    />
  );
}

export default KeyboardDatePickerFormik;
