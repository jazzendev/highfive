import React from "react";
import PropTypes from "prop-types";
import List from "@material-ui/core/List";
import ListItem from "@material-ui/core/ListItem";
import ListItemIcon from "@material-ui/core/ListItemIcon";
import ListItemText from "@material-ui/core/ListItemText";
import DashboardIcon from "@material-ui/icons/Dashboard";
import InboxIcon from "@material-ui/icons/Inbox";
import DraftsIcon from "@material-ui/icons/Drafts";
import AssignmentIcon from "@material-ui/icons/Assignment";
import Typography from "@material-ui/core/Typography";
import { Route } from "react-router";
import { NavLink } from "react-router-dom";

function ListItemLink(props) {
  const { icon, primary, to } = props;

  const renderLink = React.useMemo(
    () =>
      React.forwardRef((itemProps, ref) => (
        <NavLink to={to} ref={ref} {...itemProps} />
      )),
    [to]
  );

  return (
    <li>
      <ListItem button component={renderLink} activeClassName="Mui-selected">
        <ListItemIcon>
          {icon ? <ListItemIcon>{icon}</ListItemIcon> : null}
        </ListItemIcon>
        <ListItemText primary={primary} />
      </ListItem>
    </li>
  );
}

ListItemLink.propTypes = {
  icon: PropTypes.element,
  primary: PropTypes.string.isRequired,
  to: PropTypes.string.isRequired,
};

export const MainRoute = (
  <List>
    <Route>
      {({ location }) => (
        <Typography gutterBottom>Current route: {location.pathname}</Typography>
      )}
    </Route>

    <ListItemLink
      to="/dashboard"
      primary="Dashboard"
      icon={<DashboardIcon />}
    />
    <ListItemLink to="/tenants" primary="Tenants" icon={<InboxIcon />} />
    <ListItemLink
      to="/accounts/this-a-test-id"
      primary="Accounts"
      icon={<DraftsIcon />}
    />
    <ListItemLink to="/roles" primary="Roles" icon={<AssignmentIcon />} />
    <ListItemLink to="/login" primary="login" />
  </List>
);
