import { useState, useEffect } from "react";
import CircularProgress from "@material-ui/core/CircularProgress";
import List from "@material-ui/core/List";
import ListItem from "@material-ui/core/ListItem";
import ListItemIcon from "@material-ui/core/ListItemIcon";
import ListItemText from "@material-ui/core/ListItemText";
import ListSubheader from "@material-ui/core/ListSubheader";
import Switch from "@material-ui/core/Switch";
import { authFetch } from "Utilities/authFetch";

function PermissionSelector(props) {
  const { onSelectionChange } = props;
  const [error, setError] = useState();
  const [isLoaded, setIsLoaded] = useState(false);
  const [candidates, setCandidates] = useState([]);
  const [selected, setSelected] = useState([]);

  useEffect(() => {
    const fetchData = (id) => {
      setIsLoaded(false);
      const apiId = id ? id : "new";
      authFetch(`http://localhost:63295/api/role/${apiId}/permission`, {
        method: "GET",
      })
        .then(
          (result) => {
            setCandidates(result.data.permissionCandidates);
            const newSelected = result.data.permissionSelected.map(
              (p) => p.permissionId
            );
            setSelected(newSelected);
            onSelectionChange(newSelected);
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
          // setIsSubmitting(false);
        });
    };

    fetchData(props.id);
  }, []); // eslint-disable-line react-hooks/exhaustive-deps

  const handleToggle = (value) => () => {
    const currentIndex = selected.indexOf(value);
    const newSelected = [...selected];

    if (currentIndex === -1) {
      newSelected.push(value);
    } else {
      newSelected.splice(currentIndex, 1);
    }

    setSelected(newSelected);
    onSelectionChange(newSelected);
  };

  if (error) {
    return <div>Error: {error.message}</div>;
  } else if (!isLoaded) {
    return <CircularProgress></CircularProgress>;
  } else
    return (
      <div>
        <List subheader={<ListSubheader>Settings</ListSubheader>}>
          {candidates.map((candidate) => (
            <ListItem
              key={candidate.id}
              role={undefined}
              dense
              button
              onClick={handleToggle(candidate.id)}
            >
              <ListItemIcon>
                <Switch
                  edge="start"
                  checked={selected.indexOf(candidate.id) !== -1}
                />
              </ListItemIcon>
              <ListItemText id={candidate.id} primary={candidate.name} />
              {/* <ListItemSecondaryAction>
                <IconButton edge="end" aria-label="comments">
                  <CommentIcon />
                </IconButton>
              </ListItemSecondaryAction> */}
            </ListItem>
          ))}
        </List>
      </div>
    );
}

export default PermissionSelector;
