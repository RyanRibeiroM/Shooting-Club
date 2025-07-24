import { Outlet } from "react-router-dom";
import { SidebarProvider } from "../../Context/SidebarContext/SidebarContext";
import Sidebar from "./Sidebar/Sidebar"; // Confirme se o caminho para seu Sidebar está correto
import Topbar from "./Topbar/Topbar"; // Confirme se o caminho para seu Topbar está correto

const Layout = () => {
  return (
    // O SidebarProvider agora fica aqui, envolvendo todas as páginas internas
    <SidebarProvider>
      <div className="app-container">
        {" "}
        {/* Use sua classe CSS principal aqui */}
        <Sidebar />
        <main className="main-content">
          {" "}
          {/* Use sua classe CSS principal aqui */}
          <Topbar />
          <div className="page-content">
            {" "}
            {/* Use sua classe CSS principal aqui */}
            {/* O <Outlet /> renderizará a página específica (Home, Armas, etc.) */}
            <Outlet />
          </div>
        </main>
      </div>
    </SidebarProvider>
  );
};

export default Layout;
