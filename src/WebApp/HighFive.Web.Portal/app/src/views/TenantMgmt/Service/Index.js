import { useState, useEffect } from "react";
import { makeStyles } from "@material-ui/core/styles";
import Button from "@material-ui/core/Button";
import Dialog from "@material-ui/core/Dialog";
import CircularProgress from "@material-ui/core/CircularProgress";
import DialogActions from "@material-ui/core/DialogActions";
import DialogContent from "@material-ui/core/DialogContent";
import DialogContentText from "@material-ui/core/DialogContentText";
import DialogTitle from "@material-ui/core/DialogTitle";

import { authFetch } from "Utilities/authFetch";
import config from "config";

import Upsert from "./Upsert";

const useStyles = makeStyles((theme) => ({
  buttonLoading: {
    position: "absolute",
    top: "50%",
    left: "50%",
    marginTop: -10,
    marginLeft: -10,
  },
}));

export const Index = function (props) {
  const classes = useStyles();
  const { open, tenant, closeHandler } = props;

  const [isOpen, setIsOpen] = useState(open);

  const [error, setError] = useState();
  const [isLoaded, setIsLoaded] = useState(false);
  const [data, setData] = useState([]);

  const [isSubmitting, setIsSubmitting] = useState(false);

  useEffect(() => {
    setIsOpen(props.open);
    if (props.open) {
      if (props.tenant.id) {
        fetchData(props.tenant.id);
      } else {
        initData();
      }
    }
  }, [props.open, props.tenant]);

  const handleClose = () => {
    closeHandler();
  };

  const fetchData = (id) => {
    setIsLoaded(false);
    authFetch(`${config.apiUrl}/api/tenant/${id}/service`, {
      method: "GET",
    })
      .then(
        (result) => {
          setData(result.data);
          setError(null);
        },
        // Note: it's important to handle errors here
        // instead of a catch() block so that we don't swallow
        // exceptions from actual bugs in components.
        (error) => {
          setError(error);
        }
      )
      .then(() => {
        setIsLoaded(true);
        setIsSubmitting(false);
      });
  };

  const initData = () => {
    setIsLoaded(true);
    setIsSubmitting(false);
  };

  const handleSubmitStart = () => {
    setIsSubmitting(true);
  };

  const handleSubmitComplete = () => {
    setIsSubmitting(false);
  };

  return (
    <div>
      <Dialog
        open={isOpen}
        onClose={handleClose}
        scroll="paper"
        maxWidth="md"
        fullWidth
        aria-labelledby="form-dialog-title"
      >
        <DialogTitle id="form-dialog-title">使用服务一览</DialogTitle>

        {error ? (
          <div>Error: {error.message}</div>
        ) : !isLoaded ? (
          <div>Loading...</div>
        ) : (
          <DialogContent>
            <DialogContentText>域ID：{tenant.id}，域名称：{tenant.name}</DialogContentText>

            <div>
              {data.map((item) => (
                <Upsert
                  key={item.id}
                  values={item}
                  tenantId={tenant.id}
                  onSubmitStarted={handleSubmitStart}
                  onSubmitCompleted={handleSubmitComplete}
                ></Upsert>
              ))}
            </div>
          </DialogContent>
        )}

        <DialogActions>
          <Button onClick={handleClose} color="default">
            Cancel
          </Button>
          {!error && isLoaded && (
            <Button color="primary" disabled={isSubmitting}>
              {isSubmitting && (
                <CircularProgress
                  size={18}
                  className={classes.buttonLoading}
                ></CircularProgress>
              )}
              Submit
            </Button>
          )}
        </DialogActions>
      </Dialog>
    </div>
  );
};

// export default Index;
