import clsx from "clsx";
import { Switch, Route } from "react-router-dom";
import { makeStyles } from "@material-ui/core/styles";
import Grid from "@material-ui/core/Grid";
import Paper from "@material-ui/core/Paper";
import Container from "@material-ui/core/Container";

import TenantMgmt from "./views/TenantMgmt";
import AccountMgmt from "./views/AccountMgmt";
import RoleMgmt from "./views/RoleMgmt";
import Login from "./views/Login";
import Profile from "./views/Profile"

import Deposits from "./templates/Deposits";
import Orders from "./templates/Orders";

const useStyles = makeStyles((theme) => ({
  container: {
    paddingTop: theme.spacing(4),
    paddingBottom: theme.spacing(4),
  },
  paper: {
    padding: theme.spacing(2),
    display: "flex",
    overflow: "auto",
    flexDirection: "column",
  },
  fixedHeight: {
    height: 240,
  },
}));

export default function AppRoute() {
  const classes = useStyles();
  const fixedHeightPaper = clsx(classes.paper, classes.fixedHeight);

  return (
    <Container maxWidth="lg" className={classes.container}>
      <Switch>
        <Route path="/tenants" component={TenantMgmt}></Route>
        <Route path="/roles" component={RoleMgmt}></Route>
        
        <Route path="/accounts" component={Container}>
          <Route path="/accounts/:id" component={AccountMgmt}></Route>
        </Route>

        <Route path="/profile" component={Profile}/>
        
        <Route path="/login" component={Login}></Route>
        <Route path="/">
          <Grid container spacing={3} hidden>
            {/* Chart */}
            <Grid item xs={12} md={8} lg={9}>
              <Paper className={fixedHeightPaper}>{/* <Chart /> */}</Paper>
            </Grid>
            {/* Recent Deposits */}
            <Grid item xs={12} md={4} lg={3}>
              <Paper className={fixedHeightPaper}>
                <Deposits />
              </Paper>
            </Grid>
            {/* Recent Orders */}
            <Grid item xs={12}>
              <Paper className={classes.paper}>
                <Orders />
              </Paper>
            </Grid>
          </Grid>
        </Route>
      </Switch>
    </Container>
  );
}
