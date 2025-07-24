import { BrowserRouter, Routes, Route } from "react-router-dom";
import appRoutes from "./Routes/AppRoutes";
import Login from "./Pages/Login/Login";
import ProtectedRoute from "./Components/ProtectedRoute/ProtectedRoute";
import Layout from "./Components/Layout/Layout";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        {/* ROTA PÚBLICA: Todos podem acessar */}
        <Route path="/" element={<Login />} />

        {/* ROTAS PROTEGIDAS: A mágica acontece aqui */}
        {/* O 'ProtectedRoute' age como um guardião para todas as rotas aninhadas dentro dele. */}
        <Route element={<ProtectedRoute />}>
          {/* O 'Layout' aplica o visual (Sidebar/Topbar) a todas as rotas aninhadas. */}
          <Route element={<Layout />}>
            {appRoutes.map(({ path, element }) => (
              <Route key={path} path={path} element={element} />
            ))}
          </Route>
        </Route>

        {/* Rota para páginas não encontradas */}
        <Route path="*" element={<h1>404 - Página Não Encontrada</h1>} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
