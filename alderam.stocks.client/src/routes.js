import React, { Component } from "react";
import { BrowserRouter, Switch, Route, Redirect } from "react-router-dom";

import Main from "./pages/main/Main";

import { isAuthenticated } from './services/Auth';
import Dashboard from "./pages/dashboard/Dashboard";
import Operacoes from "./pages/operacoes/Operacoes";

const PrivateRoute = ({ component: Component, ...rest }) => (
    <Route
        {...rest}
        render={props =>
            isAuthenticated()
                ? (<Component {...props} />)
                : (<Redirect to={{ pathname: "/401", state: { from: props.location } }} />)
        }
    />
);

const Routes = () => (
    <BrowserRouter>
        <Switch>
            <Route exact path="/" component={Main}></Route>
            <Route exact path="/dashboard" component={Dashboard}></Route>
            <Route exact path="/operacoes" component={Operacoes}></Route>
            <PrivateRoute path="/blocked" component={Main}></PrivateRoute>
        </Switch>
    </BrowserRouter>
) 
export default Routes;
