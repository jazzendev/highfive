import { BehaviorSubject } from "rxjs";
import config from "config";

const currentUserSubject = new BehaviorSubject(
  JSON.parse(localStorage.getItem("user"))
);

export const authenticationService = {
  login,
  logout,
  currentUserSubject: currentUserSubject.asObservable(),
  get currentUser() {
    return currentUserSubject.value;
  },
};

function login(model, success, error, final) {
  const requestOptions = {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({
      username: model.username,
      passwordHash: btoa(model.password),
    }),
  };

  return fetch(`${config.apiUrl}/api/auth`, requestOptions)
    .then((res) => res.json())
    .then((result) => {
      if (result.data && !result.error) {
        const user = result.data;
        localStorage.setItem("user.token", user.token);
        localStorage.setItem("user", JSON.stringify(user));
        currentUserSubject.next(user);
        !!success && success();
      } else {
        !!error && error(result.error);
      }
    })
    .catch((e) => !!error && error(e))
    .then(() => {
      !!final && final();
    });
}

function logout() {
  // remove user from local storage to log user out
  localStorage.removeItem("user");
  localStorage.removeItem("user.token");
  currentUserSubject.next(null);
}
