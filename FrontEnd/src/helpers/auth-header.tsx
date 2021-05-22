import { authenticationService } from "../services/authentication.service";

export function authHeader() {
  const currentUser = authenticationService.currentUserValue;
  if (currentUser && currentUser.token) {
    const authHeader = new Headers();
    authHeader.append("Authorization", `Bearer ${currentUser.token}`);
    return authHeader;
  } else {
    return {};
  }
}
