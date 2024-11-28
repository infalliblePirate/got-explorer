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
import Layout from './pages/additional_components/Layout.tsx';

export default function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route index element={<MainPage />} />
                <Route path="/" element=<Layout />>
                    <Route path="profile" element={<ProfilePage />} />
                    <Route path="startgame" element={<StartGamePage />} />
                </Route>
                <Route path="signup" element={<SignUpPage />} />
                <Route path="login" element={<LogInPage />} />
                <Route path="forgetpass" element={<ForgetPasswordPage />} />
                <Route path="setnewpass" element={<SetNewPasswordPage />} />
            </Routes>
        </BrowserRouter>
    );
}
// @ts-expect-error
const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(<App />);


