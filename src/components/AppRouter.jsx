import React, { useContext } from "react";
import { Routes, Route, Navigate } from "react-router-dom";
import Home from "../pages/Home"
import Auth from "../pages/Auth"
import { Context } from "../index";
import { useAuthState } from 'react-firebase-hooks/auth'

export const routes = {
    AUTH: '/',
    HOME: '/home',
}

const AppRouter = () => {
    const { auth } = useContext(Context)
    const [user] = useAuthState(auth)

    return (
        <Routes>
            {
                user ?
                    <>
                        <Route path={routes.HOME} element={<Home />} />
                        <Route path="*" element={<Navigate to={routes.HOME} />} />
                    </>
                    :
                    <>
                        <Route path={routes.AUTH} element={<Auth />} />
                        <Route path="*" element={<Navigate to={routes.AUTH} />} />
                    </>
            }
        </Routes>
    )
}

export default AppRouter