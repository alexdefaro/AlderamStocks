import React, { Component } from "react";
import { BrowserRouter, Switch, Route, Redirect } from "react-router-dom";
import { authenticationService } from './services/Auth';

import Main from "./pages/main/Main";

import Dashboard from "./pages/dashboard/Dashboard";
import Operacoes from "./pages/operacoes/Operacoes";

const PrivateRoute = ({ component: Component, ...rest }) => (
    <Route
        {...rest}
        render={props =>
            authenticationService.isAuthenticated()
                ? (<Component {...props} />)
                : (<Redirect to={{ pathname: "/", state: { from: props.location } }} />)
        }
    />
);

const Routes = () => (
    <BrowserRouter>
        <Switch>
            <Route exact path="/" component={Main}></Route>
            <PrivateRoute exact path="/dashboard" component={Dashboard}></PrivateRoute>
            <PrivateRoute exact path="/operacoes" component={Operacoes}></PrivateRoute>
            <PrivateRoute path="/blocked" component={Main}></PrivateRoute>
        </Switch>
    </BrowserRouter>
) 
export default Routes;
