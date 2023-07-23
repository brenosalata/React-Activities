import { makeAutoObservable, reaction } from "mobx";
import { ServerError } from "../models/serverError";

export default class CommonStore{
    error: ServerError | null = null;
    token: string | null = localStorage.getItem('jwt');
    appLoaded = false;
    
    constructor(){
        makeAutoObservable(this);


        // this will run whenever the observable changes - it does not run when initializing
        // there are 2 types of reaction - 1 reaction is this we are using and the 2 is AutoReaction, this one runs when initializing
        reaction(
            () => this.token,
            token => {
                if (token) {
                    localStorage.setItem('jwt',token);
                } else {
                    localStorage.removeItem('jwt');
                }
            }
        )
    }

    setServerError(error: ServerError) {
        this.error = error;
    }

    // experts say we should never use localStorage to hold data such as tokens
    // instead we should use mobx or UseStore
    // BUT THIS IS A RLLY GOOD SOLUTION
    setToken = (token: string | null) =>
    {
        //BC we are using reaction we don't need to set localStorage anymore
        //whenever the token changes reaction will be called
        // if (token) localStorage.setItem('jwt', token);
        this.token = token;
    }

    setAppLoaded = () => {
        this.appLoaded = true;
    }

}