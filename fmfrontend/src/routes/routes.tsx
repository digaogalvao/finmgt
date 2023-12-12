import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { HomePage } from '../pages/HomePage';
import { RelatorioPage } from '../pages/RelatorioPage';
import { NotFoundPage } from '../pages/NotFoundPage';
import { LancamentoPage } from '../pages/LancamentoPage';

export function AppRoutes() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/lancamento" element={<LancamentoPage />} />
        <Route path="/relatorio" element={<RelatorioPage />} />
        <Route path="*" element={<NotFoundPage />} />
      </Routes>
    </BrowserRouter>
  );
}
