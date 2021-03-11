import "./App.css";
import { useState, useEffect } from "react";
import Dashboard from "./templates/Dashboard";
import SignInSide from "./templates/SignInSide";
import { authenticationService } from "Services/authentication.service";

import DateFnsUtils from '@date-io/date-fns';
import format from "date-fns/format";
import cnLocale from "date-fns/locale/zh-CN";
import {
  MuiPickersUtilsProvider,
} from "@material-ui/pickers";

import { BrowserRouter as Router, useLocation } from "react-router-dom";

function ScrollToTop() {
  const { pathname } = useLocation();

  useEffect(() => {
    window.scrollTo(0, 0);
  }, [pathname]);

  return null;
}

class LocalizedUtils extends DateFnsUtils {
  getDatePickerHeaderText(date) {
    return format(date, "yyyy-MM-dd", { locale: this.locale });
  }
}

function App() {
  const [currentUser, setCurrentUser] = useState();

  useEffect(() => {
    // subscribe to home component messages
    const subscription = authenticationService.currentUserSubject.subscribe((x) =>
      setCurrentUser(x)
    );

    // return unsubscribe method to execute when component unmounts
    return subscription.unsubscribe;
  }, []);

  return (
    <Router>
      <ScrollToTop />
      <MuiPickersUtilsProvider
        utils={LocalizedUtils}
        locale={cnLocale}>
        {currentUser ? <Dashboard></Dashboard> : <SignInSide></SignInSide>}
      </MuiPickersUtilsProvider>
    </Router>
  );
}

export default App;
