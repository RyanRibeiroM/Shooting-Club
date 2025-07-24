import React from "react";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import appRoutes from "./Routes/AppRoutes";
import Login from "./Pages/Login/Login";
import { SidebarProvider } from "./Context/SidebarContext/SidebarContext";


function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Login />} />

        <Route
          path="*"
          element={
            <SidebarProvider>
              <Routes>
                {appRoutes.map(({ path, element }) => (
                  <Route key={path} path={path} element={element} />
                ))}
              </Routes>
            </SidebarProvider>
          }
        />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
