import { authHeader } from "../helpers/auth-header";
import { handleResponse } from "../helpers/handle-response";

export const reportService = {
  getReport,
  exportExcel,
};

async function getReport() {
  const requestOptions = { method: "GET", headers: authHeader() };
  const response = await fetch(`https://localhost:5001/api/reports`, requestOptions);
  return handleResponse(response);
}

async function exportExcel() {
  const requestOptions = { method: "GET", headers: authHeader() };
  const response = await fetch(
    `https://localhost:5001/api/Reports/ExportXls`,
    requestOptions
  );
  return handleResponse(response);
}
