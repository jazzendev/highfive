import React, { useImperativeHandle, forwardRef, useState } from "react";
import TextField from "@material-ui/core/TextField";
import { makeStyles } from "@material-ui/core/styles";
import { useFormik } from "formik";
import * as Yup from "yup";
import PermissionSelector from "./PermissionSelector";
import { authFetch } from "Utilities/authFetch";

const useStyles = makeStyles((theme) => ({
  root: {
    "& .MuiTextField-root": {
      margin: theme.spacing(1),
    },
  },
}));

function Upsert(props, ref) {
  const { values, onSubmitStarted, onSubmitCompleted } = props;

  const [selectedPermissions, setSelectedPermissions] = useState([0]);
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
      description: Yup.string(),
    }),
    onSubmit: (values) => {
      onSubmitStarted();
      apiUpdate(values);
    },
  });

  const apiUpdate = (values) => {
    if (selectedPermissions[0] === 0) {
      alert("Permission Not Loaded.");
      onSubmitCompleted();
      return;
    }

    values.permissions = selectedPermissions;

    const url = `http://localhost:63295/api/role`;
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

  const handleSelectionChange = (selections) => {
    setSelectedPermissions(selections);
  };
  const classes = useStyles();

  return (
    <div>
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
          label="Name"
          onChange={formik.handleChange}
          value={formik.values.name}
          error={formik.touched.name && Boolean(formik.errors.name)}
          helperText={formik.touched.name ? formik.errors.name : ""}
          fullWidth
        />
        <TextField
          required
          name="description"
          label="Description"
          onChange={formik.handleChange}
          value={formik.values.description}
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
      <PermissionSelector
        id={formik.values.id}
        onSelectionChange={handleSelectionChange}
      ></PermissionSelector>
    </div>
  );
}

export default forwardRef(Upsert);
