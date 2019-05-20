--
-- PostgreSQL database dump
--

-- Dumped from database version 11.2
-- Dumped by pg_dump version 11.2

-- Started on 2019-05-20 16:28:10

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET client_min_messages = warning;
SET row_security = off;

DROP DATABASE sectormodel;
--
-- TOC entry 3892 (class 1262 OID 16395)
-- Name: sectormodel; Type: DATABASE; Schema: -; Owner: gbrooksman
--

CREATE DATABASE sectormodel WITH TEMPLATE = template0 ENCODING = 'UTF8' LC_COLLATE = 'en_US.UTF-8' LC_CTYPE = 'en_US.UTF-8';


ALTER DATABASE sectormodel OWNER TO gbrooksman;

\connect sectormodel

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 2 (class 3079 OID 16406)
-- Name: uuid-ossp; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS "uuid-ossp" WITH SCHEMA public;


--
-- TOC entry 3894 (class 0 OID 0)
-- Dependencies: 2
-- Name: EXTENSION "uuid-ossp"; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION "uuid-ossp" IS 'generate universally unique identifiers (UUIDs)';


--
-- TOC entry 214 (class 1255 OID 16434)
-- Name: update_updated_at(); Type: FUNCTION; Schema: public; Owner: gbrooksman
--

CREATE FUNCTION public.update_updated_at() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
  BEGIN
      NEW.updated_at = now();
      RETURN NEW;
  END;
  $$;


ALTER FUNCTION public.update_updated_at() OWNER TO gbrooksman;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 201 (class 1259 OID 16452)
-- Name: equities; Type: TABLE; Schema: public; Owner: gbrooksman
--

CREATE TABLE public.equities (
    symbol character varying(5) NOT NULL,
    name character varying(200) NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone DEFAULT now(),
    id uuid DEFAULT public.uuid_generate_v1() NOT NULL
);


ALTER TABLE public.equities OWNER TO gbrooksman;

--
-- TOC entry 203 (class 1259 OID 16469)
-- Name: equity_group_items; Type: TABLE; Schema: public; Owner: gbrooksman
--

CREATE TABLE public.equity_group_items (
    id uuid DEFAULT public.uuid_generate_v1() NOT NULL,
    group_id uuid NOT NULL,
    equity_id uuid NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone DEFAULT now() NOT NULL
);


ALTER TABLE public.equity_group_items OWNER TO gbrooksman;

--
-- TOC entry 202 (class 1259 OID 16461)
-- Name: equity_groups; Type: TABLE; Schema: public; Owner: gbrooksman
--

CREATE TABLE public.equity_groups (
    id uuid DEFAULT public.uuid_generate_v1() NOT NULL,
    name character varying(200) NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone DEFAULT now() NOT NULL,
    active boolean DEFAULT true NOT NULL
);


ALTER TABLE public.equity_groups OWNER TO gbrooksman;

--
-- TOC entry 200 (class 1259 OID 16429)
-- Name: model_equities; Type: TABLE; Schema: public; Owner: gbrooksman
--

CREATE TABLE public.model_equities (
    model_id uuid NOT NULL,
    percent integer NOT NULL,
    updated_at timestamp without time zone DEFAULT now() NOT NULL,
    created_at timestamp without time zone DEFAULT now() NOT NULL,
    id uuid DEFAULT public.uuid_generate_v1() NOT NULL,
    equity_id uuid NOT NULL
);


ALTER TABLE public.model_equities OWNER TO gbrooksman;

--
-- TOC entry 198 (class 1259 OID 16418)
-- Name: quotes; Type: TABLE; Schema: public; Owner: gbrooksman
--

CREATE TABLE public.quotes (
    date date NOT NULL,
    price money,
    volume bigint,
    created_at timestamp without time zone DEFAULT now(),
    updated_at timestamp without time zone DEFAULT now(),
    equity_id uuid NOT NULL
);


ALTER TABLE public.quotes OWNER TO gbrooksman;

--
-- TOC entry 199 (class 1259 OID 16423)
-- Name: user_models; Type: TABLE; Schema: public; Owner: gbrooksman
--

CREATE TABLE public.user_models (
    id uuid DEFAULT public.uuid_generate_v1() NOT NULL,
    user_id uuid,
    name character varying(100),
    created_at timestamp without time zone DEFAULT now(),
    updated_at timestamp without time zone DEFAULT now(),
    active boolean,
    start_date date,
    stop_date date,
    start_value money,
    stop_value money
);


ALTER TABLE public.user_models OWNER TO gbrooksman;

--
-- TOC entry 197 (class 1259 OID 16401)
-- Name: users; Type: TABLE; Schema: public; Owner: gbrooksman
--

CREATE TABLE public.users (
    user_name character varying(25) NOT NULL,
    password character varying(15),
    active boolean,
    id uuid DEFAULT public.uuid_generate_v1(),
    created_at timestamp without time zone DEFAULT now(),
    updated_at timestamp without time zone DEFAULT now()
);


ALTER TABLE public.users OWNER TO gbrooksman;

--
-- TOC entry 3750 (class 2606 OID 16456)
-- Name: equities equities_pkey; Type: CONSTRAINT; Schema: public; Owner: gbrooksman
--

ALTER TABLE ONLY public.equities
    ADD CONSTRAINT equities_pkey PRIMARY KEY (symbol);


--
-- TOC entry 3756 (class 2606 OID 16473)
-- Name: equity_group_items equity_group_items_pkey; Type: CONSTRAINT; Schema: public; Owner: gbrooksman
--

ALTER TABLE ONLY public.equity_group_items
    ADD CONSTRAINT equity_group_items_pkey PRIMARY KEY (id);


--
-- TOC entry 3752 (class 2606 OID 16465)
-- Name: equity_groups equity_groups_pkey; Type: CONSTRAINT; Schema: public; Owner: gbrooksman
--

ALTER TABLE ONLY public.equity_groups
    ADD CONSTRAINT equity_groups_pkey PRIMARY KEY (id);


--
-- TOC entry 3754 (class 2606 OID 16496)
-- Name: equity_groups ix_equity_grouips_name; Type: CONSTRAINT; Schema: public; Owner: gbrooksman
--

ALTER TABLE ONLY public.equity_groups
    ADD CONSTRAINT ix_equity_grouips_name UNIQUE (name);


--
-- TOC entry 3758 (class 2606 OID 16494)
-- Name: equity_group_items ix_group_equity_id; Type: CONSTRAINT; Schema: public; Owner: gbrooksman
--

ALTER TABLE ONLY public.equity_group_items
    ADD CONSTRAINT ix_group_equity_id UNIQUE (group_id, equity_id);


--
-- TOC entry 3746 (class 2606 OID 16498)
-- Name: model_equities ix_model_equities_model_equity; Type: CONSTRAINT; Schema: public; Owner: gbrooksman
--

ALTER TABLE ONLY public.model_equities
    ADD CONSTRAINT ix_model_equities_model_equity UNIQUE (model_id, equity_id);


--
-- TOC entry 3736 (class 2606 OID 16492)
-- Name: users ix_user_id; Type: CONSTRAINT; Schema: public; Owner: gbrooksman
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT ix_user_id UNIQUE (id);


--
-- TOC entry 3742 (class 2606 OID 16500)
-- Name: user_models ix_user_models_user_id_name; Type: CONSTRAINT; Schema: public; Owner: gbrooksman
--

ALTER TABLE ONLY public.user_models
    ADD CONSTRAINT ix_user_models_user_id_name UNIQUE (user_id, name);


--
-- TOC entry 3748 (class 2606 OID 16480)
-- Name: model_equities model_equities_pkey; Type: CONSTRAINT; Schema: public; Owner: gbrooksman
--

ALTER TABLE ONLY public.model_equities
    ADD CONSTRAINT model_equities_pkey PRIMARY KEY (id);


--
-- TOC entry 3740 (class 2606 OID 16490)
-- Name: quotes quotes_pkey; Type: CONSTRAINT; Schema: public; Owner: gbrooksman
--

ALTER TABLE ONLY public.quotes
    ADD CONSTRAINT quotes_pkey PRIMARY KEY (equity_id, date);


--
-- TOC entry 3744 (class 2606 OID 16428)
-- Name: user_models user_models_pkey; Type: CONSTRAINT; Schema: public; Owner: gbrooksman
--

ALTER TABLE ONLY public.user_models
    ADD CONSTRAINT user_models_pkey PRIMARY KEY (id);


--
-- TOC entry 3738 (class 2606 OID 16405)
-- Name: users users_pkey; Type: CONSTRAINT; Schema: public; Owner: gbrooksman
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (user_name);


--
-- TOC entry 3763 (class 2620 OID 16459)
-- Name: equities update_equities_updated_at; Type: TRIGGER; Schema: public; Owner: gbrooksman
--

CREATE TRIGGER update_equities_updated_at BEFORE UPDATE ON public.equities FOR EACH ROW EXECUTE PROCEDURE public.update_updated_at();


--
-- TOC entry 3764 (class 2620 OID 16468)
-- Name: equity_groups update_equities_updated_at; Type: TRIGGER; Schema: public; Owner: gbrooksman
--

CREATE TRIGGER update_equities_updated_at BEFORE UPDATE ON public.equity_groups FOR EACH ROW EXECUTE PROCEDURE public.update_updated_at();


--
-- TOC entry 3765 (class 2620 OID 16476)
-- Name: equity_group_items update_equity_group_items_updated_at; Type: TRIGGER; Schema: public; Owner: gbrooksman
--

CREATE TRIGGER update_equity_group_items_updated_at BEFORE UPDATE ON public.equity_group_items FOR EACH ROW EXECUTE PROCEDURE public.update_updated_at();


--
-- TOC entry 3762 (class 2620 OID 16444)
-- Name: model_equities update_model_quotes_updated_at; Type: TRIGGER; Schema: public; Owner: gbrooksman
--

CREATE TRIGGER update_model_quotes_updated_at BEFORE UPDATE ON public.model_equities FOR EACH ROW EXECUTE PROCEDURE public.update_updated_at();


--
-- TOC entry 3760 (class 2620 OID 16442)
-- Name: quotes update_quotes_updated_at; Type: TRIGGER; Schema: public; Owner: gbrooksman
--

CREATE TRIGGER update_quotes_updated_at BEFORE UPDATE ON public.quotes FOR EACH ROW EXECUTE PROCEDURE public.update_updated_at();


--
-- TOC entry 3761 (class 2620 OID 16439)
-- Name: user_models update_user_models_updated_at; Type: TRIGGER; Schema: public; Owner: gbrooksman
--

CREATE TRIGGER update_user_models_updated_at BEFORE UPDATE ON public.user_models FOR EACH ROW EXECUTE PROCEDURE public.update_updated_at();


--
-- TOC entry 3759 (class 2620 OID 16436)
-- Name: users update_users_updated_at; Type: TRIGGER; Schema: public; Owner: gbrooksman
--

CREATE TRIGGER update_users_updated_at BEFORE UPDATE ON public.users FOR EACH ROW EXECUTE PROCEDURE public.update_updated_at();


--
-- TOC entry 3893 (class 0 OID 0)
-- Dependencies: 4
-- Name: SCHEMA public; Type: ACL; Schema: -; Owner: gbrooksman
--

REVOKE ALL ON SCHEMA public FROM rdsadmin;
REVOKE ALL ON SCHEMA public FROM PUBLIC;
GRANT ALL ON SCHEMA public TO gbrooksman;
GRANT ALL ON SCHEMA public TO PUBLIC;


-- Completed on 2019-05-20 16:28:27

--
-- PostgreSQL database dump complete
--

