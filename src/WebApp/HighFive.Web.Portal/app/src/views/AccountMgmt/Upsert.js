import React, {
  useImperativeHandle,
  forwardRef,
  useState,
  useEffect,
  useRef,
} from "react";
import Box from "@material-ui/core/Box";
import Grid from "@material-ui/core/Grid";
import TextField from "@material-ui/core/TextField";
import List from "@material-ui/core/List";
import ListItem from "@material-ui/core/ListItem";
import ListItemIcon from "@material-ui/core/ListItemIcon";
import ListItemText from "@material-ui/core/ListItemText";
import Typography from "@material-ui/core/Typography";
import Switch from "@material-ui/core/Switch";
import CircularProgress from "@material-ui/core/CircularProgress";

import KeyboardDatePickerFormik from "Utilities/KeyboardDatePicker";

import { makeStyles } from "@material-ui/core/styles";
import { Formik, useFormikContext } from "formik";
import * as yup from "yup";
import { authFetch } from "Utilities/authFetch";
import config from "config";

const useStyles = makeStyles((theme) => ({
  root: {
    "& .MuiTextField-root": {
      marginBottom: theme.spacing(1),
    },
  },
}));

function Upsert(props, ref) {
  const { values, onSubmitStarted, onSubmitCompleted } = props;
  const formikRef = useRef();

  const [model, setModel] = useState(values);
  const [isRoleLoaded, setIsRoleLoaded] = useState(false);
  const [roles, setRoles] = useState([]);
  const [selectedRoles, setSelectedRoles] = useState([0]);
  const [error, setError] = useState();

  useEffect(() => {
    const fetchRoles = (id) => {
      setIsRoleLoaded(false);
      authFetch(`${config.apiUrl}/api/role`, {
        method: "GET",
      })
        .then(
          (result) => {
            setRoles(result.data);
            setModel(values);
            const newSelected = values.roles
              ? values.roles.map((r) => r.roleId)
              : [];
            setSelectedRoles(newSelected);
          },
          // Note: it's important to handle errors here
          // instead of a catch() block so that we don't swallow
          // exceptions from actual bugs in components.
          (error) => {
            setError(error);
          }
        )
        .then(() => {
          setIsRoleLoaded(true);
        });
    };

    fetchRoles();
  }, []);

  useImperativeHandle(ref, () => ({
    submit: () => {
      formikRef.current.submitForm();
    },
  }));

  const handleToggle = (value) => () => {
    const currentIndex = selectedRoles.indexOf(value);
    const newSelected = [...selectedRoles];

    if (currentIndex === -1) {
      newSelected.push(value);
    } else {
      newSelected.splice(currentIndex, 1);
    }

    setSelectedRoles(newSelected);
  };

  const apiUpdate = (values) => {
    if (selectedRoles[0] === 0) {
      alert("Roles Not Loaded.");
      onSubmitCompleted();
      return;
    }

    values.roles = selectedRoles.map((s) => {
      return { roleId: s };
    });

    const url = `${config.apiUrl}/api/account`;
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
          formikRef.current.setValues(result.data);
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

  if (error) {
    return <div>Error: {error.message}</div>;
  } else {
    return (
      <Formik
        enableReinitialize
        innerRef={formikRef}
        initialValues={model}
        validationSchema={yup.object({
          //   id: yup.string(),
          username: yup.string().required("Required"),
          name: yup.string().required("Required"),
          email: yup.string().email().nullable(),
          mobile: yup
            .string()
            .matches(/^[0-9]{11}$/, "Wrong Mobile Format.")
            .nullable(),
          accessStartTime: yup.date().nullable(),
          accessEndTime: yup
            .date()
            .nullable()
            .when("accessStartTime", (startTime, schema) => {
              return startTime && new Date(startTime)
                ? schema.min(startTime, "必须大于起始时间。")
                : schema.nullable();
            }),
          blockStartTime: yup.date().nullable(),
          blockEndTime: yup
            .date()
            .nullable()
            .when("blockStartTime", (startTime, schema) => {
              return startTime && new Date(startTime)
                ? schema.min(startTime, "必须大于起始时间。")
                : schema.nullable();
            }),
        })}
        onSubmit={(values) => {
          onSubmitStarted();
          apiUpdate(values);
        }}
      >
        {(props) => (
          <form
            className={classes.root}
            onSubmit={props.onSubmit}
            autoComplete="off"
          >
            <TextField
              name="id"
              label="Id (readonly)"
              onChange={props.handleChange}
              value={props.values.id}
              error={props.touched.id && Boolean(props.errors.id)}
              helperText={props.touched.id ? props.errors.id : ""}
              InputProps={{ readOnly: true }}
              fullWidth
            />
            <TextField
              required
              name="username"
              label="username"
              onChange={props.handleChange}
              value={props.values.username}
              error={props.touched.username && Boolean(props.errors.username)}
              helperText={props.touched.username ? props.errors.username : ""}
              fullWidth
            />
            <TextField
              required
              name="name"
              label="name"
              onChange={props.handleChange}
              value={props.values.name}
              error={props.touched.name && Boolean(props.errors.name)}
              helperText={props.touched.name ? props.errors.name : ""}
              fullWidth
            />
            <TextField
              name="mobile"
              label="mobile"
              onChange={props.handleChange}
              value={props.values.mobile}
              error={props.touched.mobile && Boolean(props.errors.mobile)}
              helperText={props.touched.mobile ? props.errors.mobile : ""}
              fullWidth
            />
            <TextField
              name="email"
              label="email"
              onChange={props.handleChange}
              value={props.values.email}
              error={props.touched.email && Boolean(props.errors.email)}
              helperText={props.touched.email ? props.errors.email : ""}
              fullWidth
            />
            <Grid
              container
              direction="row"
              justify="center"
              alignItems="baseline"
            >
              <KeyboardDatePickerFormik
                component={Grid}
                item
                md={3}
                xs={6}
                clearable
                format="yyyy-MM-dd"
                label="Access Begin"
                name="accessStartTime"
                value={props.values.accessStartTime}
                onChange={props.handleChange}
                error={
                  props.touched.accessStartTime &&
                  Boolean(props.errors.accessStartTime)
                }
                helperText={
                  props.touched.accessStartTime
                    ? props.errors.accessStartTime
                    : ""
                }
              />
              <KeyboardDatePickerFormik
                component={Grid}
                item
                md={3}
                xs={6}
                clearable
                format="yyyy-MM-dd"
                label="Access End"
                name="accessEndTime"
                value={props.values.accessEndTime}
                onChange={props.handleChange}
                error={
                  props.touched.accessEndTime &&
                  Boolean(props.errors.accessEndTime)
                }
                helperText={
                  props.touched.accessEndTime ? props.errors.accessEndTime : ""
                }
              />
              <KeyboardDatePickerFormik
                component={Grid}
                item
                md={3}
                xs={6}
                clearable
                format="yyyy-MM-dd"
                label="Block Begin"
                name="blockStartTime"
                value={props.values.blockStartTime}
                onChange={props.handleChange}
                error={
                  props.touched.blockStartTime &&
                  Boolean(props.errors.blockStartTime)
                }
                helperText={
                  props.touched.blockStartTime
                    ? props.errors.blockStartTime
                    : ""
                }
              />
              <KeyboardDatePickerFormik
                component={Grid}
                item
                md={3}
                xs={6}
                clearable
                format="yyyy-MM-dd"
                label="Block End"
                name="blockEndTime"
                value={props.values.blockEndTime}
                onChange={props.handleChange}
                error={
                  props.touched.blockEndTime &&
                  Boolean(props.errors.blockEndTime)
                }
                helperText={
                  props.touched.blockEndTime ? props.errors.blockEndTime : ""
                }
              />
            </Grid>

            {error && error.message ? <div>{error.message}</div> : <div></div>}

            <Box mt={2}>
              <Typography variant="h6">Roles</Typography>
            </Box>

            {!isRoleLoaded ? (
              <CircularProgress></CircularProgress>
            ) : (
              <List>
                {roles.map((role) => (
                  <ListItem
                    key={role.id}
                    role={undefined}
                    disableGutters
                    dense
                    button
                    onClick={handleToggle(role.id)}
                  >
                    <ListItemIcon>
                      <Switch
                        edge="start"
                        checked={selectedRoles.indexOf(role.id) !== -1}
                      />
                    </ListItemIcon>
                    <ListItemText id={role.id} primary={role.name} />
                  </ListItem>
                ))}
              </List>
            )}
          </form>
        )}
      </Formik>
    );
  }
}

export default forwardRef(Upsert);
