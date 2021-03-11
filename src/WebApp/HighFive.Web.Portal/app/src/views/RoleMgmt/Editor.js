import { useState, useEffect, useRef } from "react";
import { makeStyles } from "@material-ui/core/styles";
import Button from "@material-ui/core/Button";
import Dialog from "@material-ui/core/Dialog";
import CircularProgress from "@material-ui/core/CircularProgress";
import DialogActions from "@material-ui/core/DialogActions";
import DialogContent from "@material-ui/core/DialogContent";
import DialogContentText from "@material-ui/core/DialogContentText";
import DialogTitle from "@material-ui/core/DialogTitle";
import Upsert from "./Upsert";
import { authFetch } from "Utilities/authFetch";

const useStyles = makeStyles((theme) => ({
  buttonLoading: {
    position: "absolute",
    top: "50%",
    left: "50%",
    marginTop: -10,
    marginLeft: -10,
  },
}));

function Editor(props) {
  const classes = useStyles();
  const { open, id, closeHandler, refreshHandler } = props;
  const formRef = useRef();

  const [isOpen, setIsOpen] = useState(open);

  const [error, setError] = useState();
  const [isLoaded, setIsLoaded] = useState(false);
  const [data, setData] = useState([]);

  const [isSubmitting, setIsSubmitting] = useState(false);

  useEffect(() => {
    setIsOpen(props.open);
    if (props.open) {
      if (props.id) {
        fetchData(props.id);
      } else {
        initData();
      }
    }
  }, [props.open, props.id]);

  const handleClose = () => {
    closeHandler();
  };

  const fetchData = (id) => {
    setIsLoaded(false);
    authFetch(`http://localhost:63295/api/role/${id}`, {
      method: "GET",
    })
      .then(
        (result) => {
          setData(result.data);
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
    setData({
      id: "",
      name: "",
      domain: "",
    });
  };

  const submit = () => {
    formRef.current.submit();
  };

  const handleSubmitStart = () => {
    setIsSubmitting(true);
  };

  const handleSubmitComplete = () => {
    setIsSubmitting(false);
    refreshHandler();
  };

  return (
    <div>
      <Dialog
        open={isOpen}
        onClose={handleClose}
        scroll="paper"
        maxWidth="lg"
        fullWidth
        aria-labelledby="form-dialog-title"
      >
        <DialogTitle id="form-dialog-title">{id}</DialogTitle>

        {error ? (
          <div>Error: {error.message}</div>
        ) : !isLoaded ? (
          <div>Loading...</div>
        ) : (
          <DialogContent>
            <DialogContentText>
              To subscribe to this website, please enter your email address
              here. We will send updates occasionally.
            </DialogContentText>
            <Upsert
              values={data}
              ref={formRef}
              onSubmitStarted={handleSubmitStart}
              onSubmitCompleted={handleSubmitComplete}
            ></Upsert>
          </DialogContent>
        )}

        <DialogActions>
          <Button onClick={handleClose} color="default">
            Cancel
          </Button>
          {!error && isLoaded && (
            <Button onClick={submit} color="primary" disabled={isSubmitting}>
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
}

export default Editor;
