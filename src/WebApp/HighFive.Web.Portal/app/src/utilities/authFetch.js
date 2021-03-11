import { authenticationService } from "Services/authentication.service";

export const authFetch = (url, requestOptions) => {
  if (
    authenticationService.currentUser &&
    authenticationService.currentUser.token
  ) {
    requestOptions = {
      method: requestOptions.method,
      headers: {
        ...requestOptions.headers,
        authorization: `bearer ${authenticationService.currentUser.token}`,
      },
      body: requestOptions.body,
    };

    return fetch(url, requestOptions).then((res) => res.json());
  } else {
    return Promise.reject(new Error("No user login."));
  }
};
