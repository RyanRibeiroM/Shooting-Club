import { apiFetch } from "../Api/Api";

export async function getDashboardData() {
  return await apiFetch("/dashboard", {
    method: "GET",
  });
}
