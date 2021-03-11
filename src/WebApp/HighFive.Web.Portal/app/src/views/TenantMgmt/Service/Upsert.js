import React, { useState } from "react";
import Grid from "@material-ui/core/Grid";
import Typography from "@material-ui/core/Typography";
import Paper from "@material-ui/core/Paper";
import Box from "@material-ui/core/Box";
import Button from "@material-ui/core/Button";
import ButtonGroup from "@material-ui/core/ButtonGroup";
import InputBase from "@material-ui/core/InputBase";
import KeyboardDatePickerFormik from "Utilities/KeyboardDatePicker";

import SaveIcon from "@material-ui/icons/Save";
import CancelIcon from "@material-ui/icons/Cancel";
import DoneIcon from "@material-ui/icons/Done";
import BlockIcon from "@material-ui/icons/Block";
import CloseIcon from '@material-ui/icons/Close';

import red from "@material-ui/core/colors/red";
import green from "@material-ui/core/colors/green";
import grey from "@material-ui/core/colors/grey";

import { parseISO, format } from "date-fns";
import { makeStyles } from "@material-ui/core/styles";
import { Formik } from "formik";
import * as yup from "yup";
import { authFetch } from "Utilities/authFetch";
import config from "config";

const useStyles = makeStyles((theme) => ({
  root: {
    padding: theme.spacing(1),
    marginBottom: theme.spacing(1),
  },
  form: {
    "& .MuiTextField-root": {
      margin: theme.spacing(1),
    },
  },
  paragraph: {
    "&, & .MuiTypography-root, & .MuiBox-root": {
      display: "inline-flex",
      alignItems: "center",
    },
  },
  danger: {
    color: red[500],
  },
  success: {
    color: green[500],
  },
}));

function Upsert(props) {
  const { values, tenantId } = props;
  if (!values.tenantId) {
    values.tenantId = tenantId;
  }

  const [model, setModel] = useState(values);
  const [isEditing, setIsEditing] = useState(false);
  const [error, setError] = useState();

  const apiUpdate = (values) => {
    const url = `${config.apiUrl}/api/tenant/${values.tenantId}/service`;
    // const method = !!values.id ? "POST" : "PUT";
    authFetch(url, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(values),
    })
      .then(
        (result) => {
          setModel(result.data);
          setIsEditing(false);
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
    <Paper variant="outlined" className={classes.root}>
      <Typography variant="h6" gutterBottom>
        {model.serviceCodeDescription}
      </Typography>
      <Formik
        enableReinitialize
        initialValues={model}
        validationSchema={yup.object({
          //   id: Yup.string(),
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
          // onSubmitStarted();
          // apiUpdate(values);
          apiUpdate(values);
        }}
      >
        {(props) =>
          isEditing ? (
            <form
              className={classes.form}
              onSubmit={props.handleSubmit}
              autoComplete="off"
            >
              <Grid
                container
                direction="row"
                justify="center"
                alignItems="baseline"
              >
                <KeyboardDatePickerFormik
                  component={Grid}
                  item
                  md={2}
                  xs={5}
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
                  md={2}
                  xs={5}
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
                    props.touched.accessEndTime
                      ? props.errors.accessEndTime
                      : ""
                  }
                />
                <KeyboardDatePickerFormik
                  component={Grid}
                  item
                  md={2}
                  xs={5}
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
                  md={2}
                  xs={5}
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

                <ButtonGroup
                  container
                  justify="center"
                  component={Grid}
                  item
                  md={3}
                  xs={12}
                >
                  <Button
                    variant="contained"
                    color="secondary"
                    size="small"
                    className={classes.button}
                    startIcon={<CancelIcon />}
                    onClick={() => {
                      props.resetForm();
                      setIsEditing(false);
                    }}
                  >
                    取消
                  </Button>
                  <Button
                    variant="contained"
                    color="primary"
                    size="small"
                    className={classes.button}
                    startIcon={<SaveIcon />}
                    type="submit"
                  >
                    保存
                  </Button>
                </ButtonGroup>
              </Grid>
              {error && error.message && <div>{error.message}</div>}
            </form>
          ) : (
            <Grid
              container
              direction="row"
              justify="space-between"
              alignItems="baseline"
            >
              <Typography className={classes.paragraph}>
                状态：
                {(props.values.isBlocked && (
                  <Box component="span" color={red[500]}>
                    <BlockIcon />
                    已禁用
                  </Box>
                )) ||
                  (props.values.isAllowed && (
                    <Box component="span" color={green[500]}>
                      <DoneIcon />
                      已激活
                    </Box>
                  )) || (
                    <Box component="span" color={grey[500]}>
                      <CloseIcon />
                      未激活
                    </Box>
                  )}
              </Typography>
              <Typography className={classes.paragraph}>
                有效期限：
                {(props.values.accessStartTime &&
                  format(
                    new Date(props.values.accessStartTime),
                    "yyyy年MM月dd日"
                  )) ||
                  "未设置"}{" "}
                ~{" "}
                {(props.values.accessEndTime &&
                  format(
                    new Date(props.values.accessEndTime),
                    "yyyy年MM月dd日"
                  )) ||
                  "未设置"}
              </Typography>
              <Typography className={classes.paragraph}>
                屏蔽期限：
                {(props.values.blockStartTime &&
                  format(
                    new Date(props.values.blockStartTime),
                    "yyyy年MM月dd日"
                  )) ||
                  "未设置"}{" "}
                ~{" "}
                {(props.values.blockEndTime &&
                  format(
                    new Date(props.values.blockEndTime),
                    "yyyy年MM月dd日"
                  )) ||
                  "未设置"}
              </Typography>
              <Typography className={classes.paragraph}>
                <Button
                  variant="outlined"
                  size="small"
                  className={classes.button}
                  onClick={() => setIsEditing(true)}
                >
                  编辑
                </Button>
              </Typography>
            </Grid>
          )
        }
      </Formik>
    </Paper>
  );
}

export default Upsert;
