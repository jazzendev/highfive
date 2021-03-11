import React, { useImperativeHandle, forwardRef, useState } from "react";
import TextField from "@material-ui/core/TextField";
import { makeStyles } from "@material-ui/core/styles";
import { useFormik } from "formik";
import * as Yup from "yup";
import { authFetch } from "Utilities/authFetch";
import config from "config";

const useStyles = makeStyles((theme) => ({
  root: {
    "& .MuiTextField-root": {
      margin: theme.spacing(1),
    },
  },
}));

function Info(props) {
  const { values, onSubmitStarted, onSubmitCompleted } = props;
  const [model] = useState(values);
  const [error, setError] = useState();

  const formik = useFormik({
    initialValues: model,
    validationSchema: Yup.object({
      //   id: Yup.string(),
      name: Yup.string().required("Required"),
      description: Yup.string(),
    }),
    onSubmit: (values) => {
      onSubmitStarted();
      apiUpdate(values);
    },
  });

  const apiUpdate = (values) => {
    const url = `${config.apiUrl}/api/role`;
    const method = !!values.id ? "POST" : "PUT";
    authFetch(url, {
      method: method,
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(values),
    })
      .then(
        (result) => {
          formik.setValues(result.data);
          onSubmitCompleted();
        },
        // Note: it's important to handle errors here
        // instead of a catch() block so that we don't swallow
        // exceptions from actual bugs in components.
        (error) => {
          setError(error);
        }
      )
      // .then(() => setIsLoading(false))
      .catch((error) => setError(error));
  };

  return <div></div>;
}

export default Info;
