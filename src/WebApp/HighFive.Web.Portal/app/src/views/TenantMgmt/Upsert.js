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

function Upsert(props, ref) {
  const { values, onSubmitStarted, onSubmitCompleted } = props;

  const [model] = useState(values);
  const [error, setError] = useState();

  useImperativeHandle(ref, () => ({
    submit: () => {
      formik.submitForm();
    },
  }));

  const formik = useFormik({
    initialValues: model,
    validationSchema: Yup.object({
      //   id: Yup.string(),
      name: Yup.string().required("Required"),
      domain: Yup.string().required("Required").matches(
        /\w+/, ///[a-zA-Z0-9_.-]+([.][a-zA-Z0-9_.-]+)+/,
        "Wrong Domain Format."
      ),
    }),
    onSubmit: (values) => {
      onSubmitStarted();
      apiUpdate(values);
    },
  });

  const apiUpdate = (values) => {
    const url = `${config.apiUrl}/api/tenant`;
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

  const classes = useStyles();

  return (
    <form
      className={classes.root}
      onSubmit={formik.onSubmit}
      autoComplete="off"
    >
      <TextField
        name="id"
        label="Id (readonly)"
        onChange={formik.handleChange}
        value={formik.values.id}
        error={formik.touched.id && Boolean(formik.errors.id)}
        helperText={formik.touched.id ? formik.errors.id : ""}
        InputProps={{ readOnly: true }}
        fullWidth
      />

      <TextField
        required
        name="name"
        label="name"
        onChange={formik.handleChange}
        value={formik.values.name}
        error={formik.touched.name && Boolean(formik.errors.name)}
        helperText={formik.touched.name ? formik.errors.name : ""}
        fullWidth
      />
      <TextField
        required
        name="domain"
        label="domain"
        onChange={formik.handleChange}
        value={formik.values.domain}
        error={formik.touched.domain && Boolean(formik.errors.domain)}
        helperText={formik.touched.domain ? formik.errors.domain : ""}
        fullWidth
      />

      {error && error.message ? <div>{error.message}</div> : <div></div>}
      {/* <TextField
                id="email"
                name="email"
                label="Email"
                fullWidth
            />
            <TextField
                id="password"
                name="password"
                label="Password"
                fullWidth
                type="password"
            />
            <TextField
                id="confirmPassword"
                name="confirmPassword"
                label="Confirm Password"
                fullWidth
                type="password"
            /> */}
    </form>
  );
}

export default forwardRef(Upsert);
