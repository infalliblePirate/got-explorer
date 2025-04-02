/* eslint-disable @typescript-eslint/ban-ts-comment */

import { BrowserRouter, Route, Routes } from 'react-router-dom';
import './index.scss'
import MainPage from './pages/homepage/MainPage.tsx';
import ReactDOM from 'react-dom';
import SignUpPage from './pages/auth/SignUpPage.tsx';
import LogInPage from './pages/auth/LogInPage.tsx';
import ForgetPasswordPage from './pages/auth/ForgetPassword.tsx';
import SetNewPasswordPage from './pages/auth/SetNewPassword.tsx';
import ProfilePage from './pages/auth/ProfilePage.tsx';
import StartGamePage from './pages/games/StartGamePage.tsx';
import "./index.scss";
import "@fortawesome/fontawesome-free/css/all.min.css";
import GameLevelPage from './pages/games/GameLevelPage.tsx';
import NotFoundPage from './pages/additional_components/NotFoundPage.tsx';
import { Toaster } from 'sonner';
import { GoogleOAuthProvider } from '@react-oauth/google';
import DailyGamePage from './pages/games/DailyGamePage.tsx';
import DemoGamePage from './pages/games/DemoGamePage.tsx';

export default function App() {
    return (
        <GoogleOAuthProvider clientId={process.env.CLIENT_GOOGLE_ID as string}>
            <BrowserRouter>
                <Routes>
                    <Route index element={<MainPage />} />
                    <Route path="profile" element={<ProfilePage />} />
                    <Route path="startgame" element={<StartGamePage />} />
                    <Route path="signup" element={<SignUpPage />} />
                    <Route path="login" element={<LogInPage />} />
                    <Route path="forgetpass" element={<ForgetPasswordPage />} />
                    <Route path="setnewpass" element={<SetNewPasswordPage />} />
                    <Route path="lvl/game" element={<GameLevelPage />} />
                    <Route path="lvl/dailygame" element={<DailyGamePage />} />
                    <Route path="lvl/demogame" element={<DemoGamePage />} />
                    <Route path="*" element={<NotFoundPage />} />
                </Routes>
                <Toaster position="top-center" /> { }
            </BrowserRouter>
        </GoogleOAuthProvider>
    );
}

// @ts-expect-error
const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(<App />);
